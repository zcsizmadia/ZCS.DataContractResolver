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
                return true;

            Type type = obj.GetType();

            if (!type.IsValueType)
            {
                return false;
            }

            object defaultObj = Activator.CreateInstance(type);
            if (defaultObj is null)
            {
                return false;
            }

            return defaultObj.Equals(obj);
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

                JsonPropertyInfo jsonPropertyInfo;

                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    FieldInfo fieldInfo = (FieldInfo)memberInfo;
                    jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(fieldInfo.FieldType, attr?.Name ?? fieldInfo.Name);
                    jsonPropertyInfo.Get = fieldInfo.GetValue;
                    jsonPropertyInfo.Set = (obj, value) => fieldInfo.SetValue(obj, value);
                }
                else
                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                    jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(propertyInfo.PropertyType, attr?.Name ?? propertyInfo.Name);
                    jsonPropertyInfo.Get = propertyInfo.CanRead ? propertyInfo.GetValue : null;
                    jsonPropertyInfo.Set = propertyInfo.CanWrite ? (obj, value) => propertyInfo.SetValue(obj, value) : null;
                }
                else
                {
                    continue;
                }

                if (attr != null)
                {
                    jsonPropertyInfo.Order = attr.Order;
                    jsonPropertyInfo.ShouldSerialize = !attr.EmitDefaultValue ? ((_, obj) => !IsNullOrDefault(obj)) : null;
                }

                yield return jsonPropertyInfo;
            }
        }

        public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = JsonTypeInfo.CreateJsonTypeInfo(type, options);

            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
            {
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(type)!;

                foreach (var jsonPropertyInfo in CreateDataMembers(jsonTypeInfo).OrderBy((x) => x.Order))
                {
                    jsonTypeInfo.Properties.Add(jsonPropertyInfo);
                }
            }

            return jsonTypeInfo;
        }
    }
}