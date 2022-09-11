using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Text.Json.Serialization.Metadata
{
    public class DataContractResolver : IJsonTypeInfoResolver
    {
        public static DataContractResolver Default = new DataContractResolver();

        private bool IsNullOrDefault(object obj)
        {
            if (obj is null)
            {
                return true;
            }

            Type type = obj.GetType();

            return type.IsValueType ? FormatterServices.GetUninitializedObject(type).Equals(obj) : false;
        }

        private IEnumerable<MemberInfo> EnumerateFieldsAndProperties(Type type, BindingFlags bindingFlags)
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

        private IEnumerable<JsonPropertyInfo> CreateDataMembers(JsonTypeInfo jsonTypeInfo)
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

                JsonPropertyInfo jsonPropertyInfo = null;

                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    FieldInfo fieldInfo = memberInfo as FieldInfo;
                    jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(fieldInfo.FieldType, attr?.Name ?? fieldInfo.Name);
                    jsonPropertyInfo.Get = fieldInfo.GetValue;
                    jsonPropertyInfo.Set = (obj, value) => fieldInfo.SetValue(obj, value);
                }
                else
                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                    jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(propertyInfo.PropertyType, attr?.Name ?? propertyInfo.Name);
                    if (propertyInfo.CanRead)
                    {
                        jsonPropertyInfo.Get = propertyInfo.GetValue;
                    }
                    if (propertyInfo.CanWrite)
                    {
                        jsonPropertyInfo.Set = (obj, value) => propertyInfo.SetValue(obj, value);
                    }
                }
                
                if (attr != null)
                {
                    jsonPropertyInfo.Order = attr.Order;
                    jsonPropertyInfo.ShouldSerialize = !attr.EmitDefaultValue ? ((_, obj) => !IsNullOrDefault(obj)) : null;
                }

                yield return jsonPropertyInfo;
            }
        }

        public JsonTypeInfo GetTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
            {
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type)!;

                foreach (var jsonPropertyInfo in CreateDataMembers(jsonTypeInfo).OrderBy((x) => x.Order))
                {
                    jsonTypeInfo.Properties.Add(jsonPropertyInfo);
                }
            }

            return jsonTypeInfo;
        }

        public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = JsonTypeInfo.CreateJsonTypeInfo(type, options);

            return GetTypeInfo(jsonTypeInfo);
        }
    }
}