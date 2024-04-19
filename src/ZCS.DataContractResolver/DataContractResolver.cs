﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Text.Json.Serialization.Metadata
{
    public class DataContractResolver : DefaultJsonTypeInfoResolver
    {
        private static Lazy<DataContractResolver> s_defaultInstance = new(() => new DataContractResolver());

        public static DataContractResolver Default => s_defaultInstance.Value;

        private static bool IsNullOrDefault(object obj)
        {
            if (obj is null)
            {
                return true;
            }

            Type type = obj.GetType();

            if (!type.IsValueType)
            {
                return false;
            }

            return Activator.CreateInstance(type).Equals(obj);
        }

        private static IEnumerable<MemberInfo> EnumerateFieldsAndProperties(Type type, BindingFlags bindingFlags)
        {
            foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
            {
                yield return fieldInfo;
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
            {
                yield return propertyInfo;
            }
        }

        private static IEnumerable<JsonPropertyInfo> CreateDataMembers(JsonTypeInfo jsonTypeInfo)
        {
            bool isDataContract = jsonTypeInfo.Type.GetCustomAttribute<DataContractAttribute>() != null;
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            if (isDataContract)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }

            foreach (MemberInfo memberInfo in EnumerateFieldsAndProperties(jsonTypeInfo.Type, bindingFlags))
            {
                DataMemberAttribute attr = null;
                if (isDataContract)
                {
                    attr = memberInfo.GetCustomAttribute<DataMemberAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }
                }
                else
                {
                    if (memberInfo.GetCustomAttribute<IgnoreDataMemberAttribute>() != null)
                    {
                        continue;
                    }
                }

                if (memberInfo == null)
                {
                    continue;
                }

                Func<object, object> getValue = null;
                Action<object, object> setValue = null;
                Type propertyType = null;
                string propertyName = null;

                if (memberInfo.MemberType == MemberTypes.Field && memberInfo is FieldInfo fieldInfo)
                {
                    propertyName = attr?.Name ?? fieldInfo.Name;
                    propertyType = fieldInfo.FieldType;
                    getValue = fieldInfo.GetValue;
                    setValue = fieldInfo.SetValue;
                }
                else
                if (memberInfo.MemberType == MemberTypes.Property && memberInfo is PropertyInfo propertyInfo)
                {
                    propertyName = attr?.Name ?? propertyInfo.Name;
                    propertyType = propertyInfo.PropertyType;
                    if (propertyInfo.CanRead)
                    {
                        getValue = propertyInfo.GetValue;
                    }
                    if (propertyInfo.CanWrite)
                    {
                        setValue = propertyInfo.SetValue;
                    }
                }
                else
                {
                    continue;
                }

                JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(propertyType, propertyName);
                if (jsonPropertyInfo == null)
                {
                    continue;
                }

                jsonPropertyInfo.Get = getValue;
                jsonPropertyInfo.Set = setValue;

                if (attr != null)
                {
                    jsonPropertyInfo.IsRequired = attr.IsRequired;
                    jsonPropertyInfo.Order = attr.Order;
                    jsonPropertyInfo.ShouldSerialize = !attr.EmitDefaultValue ? ((_, obj) => !IsNullOrDefault(obj)) : null;
                }
                
                if (!jsonPropertyInfo.IsRequired)
                {
                    var requiredAttr = memberInfo.GetCustomAttribute<RequiredAttribute>();
                    if (requiredAttr != null)
                    {
                        jsonPropertyInfo.IsRequired = true;
                    }
                }

                yield return jsonPropertyInfo;
            }
        }

        public static JsonTypeInfo GetTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
            {
                foreach (var jsonPropertyInfo in CreateDataMembers(jsonTypeInfo).OrderBy((x) => x.Order))
                {
                    jsonTypeInfo.Properties.Add(jsonPropertyInfo);
                }
            }

            return jsonTypeInfo;
        }

        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            {
                return jsonTypeInfo;
            }

            jsonTypeInfo.Properties.Clear();

            return GetTypeInfo(jsonTypeInfo);
        }
    }
}