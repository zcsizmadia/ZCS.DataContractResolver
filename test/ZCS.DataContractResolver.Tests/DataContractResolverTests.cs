using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace ZCS.DataContractResolver.Tests
{
    public class Person
    {
        public string FullName;
        public int Age;
    }

    public class PersonWithNonPublicMember
    {
        public string FullName;
        protected int Age = 21;
    }

    public class PersonWithoutContractWithDataMember
    {
        [DataMember(Name ="full_name")]
        public string FullName { get; set; }

        [IgnoreDataMember]
        public int Age { get; set; }
    }

    public class PersonWithIgnore
    {
        public string FullName { get; set; }

        [IgnoreDataMember]
        public int Age { get; set; }
    }

    public class PersonGetSet
    {
        public string FullName { get; set; }
        public int Age { get; set; }
    }

    [DataContract]
    public class PersonContract
    {
        [DataMember]
        public string FullName;

        [DataMember(EmitDefaultValue = false)]
        public int Age;
    }

    [DataContract]
    public class PersonContractMemberGetter
    {
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int Age { get; } = 21;
    }

    [DataContract]
    public class PersonContractWithNonPublicMember
    {
        [DataMember]
        public string FullName;

        [DataMember(EmitDefaultValue = false)]
        protected int Age = 21;
    }

    [DataContract]
    public class PersonContractWithoutDataMember
    {
        public string FullName;
        protected int Age = 21;
    }

    [DataContract]
    public class PersonContractOverrideName
    {
        [DataMember(Name = "full_name", EmitDefaultValue = false)]
        public string FullName { get; set; }

        [DataMember(Name = "age", EmitDefaultValue = false)]
        public int Age { get; set; }
    }

    [DataContract]
    public class PersonContractOrdered
    {
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string FullName { get; set; }

        [DataMember(Order = 1)]
        public int Age { get; set; }
    }

    [DataContract]
    public class PersonWithDictionary
    {
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string FullName { get; set; }

        [DataMember(Order = 1)]
        public int Age { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, string> Dict { get; set; }
    }

    [DataContract]
    public class PersonContractWithIgnore
    {
        [DataMember(EmitDefaultValue = false)]
        public string FullName { get; set; }

        [IgnoreDataMember]
        public int Age { get; set; }
    }

    [DataContract]
    public class PersonContractWithStruct
    {
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime LastLogin { get; set; }
    }

    [DataContract]
    public class PersonContractWithNullable
    {
        [DataMember(EmitDefaultValue = false)]
        public string FullName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? Age { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? LastLogin { get; set; }
    }
    public class DataContractResolverTests
    {
        private static System.Collections.IEnumerable TestCases()
        {
            yield return new TestCaseData(new Person());
            yield return new TestCaseData(new Person() { FullName = "John Doe" });
            yield return new TestCaseData(new Person() { Age = 21 });
            yield return new TestCaseData(new Person() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonWithNonPublicMember());
            yield return new TestCaseData(new PersonWithNonPublicMember() { FullName = "John Doe" });

            yield return new TestCaseData(new PersonContractWithoutDataMember());
            yield return new TestCaseData(new PersonContractWithoutDataMember() { FullName = "John Doe" });

            yield return new TestCaseData(new PersonWithIgnore());
            yield return new TestCaseData(new PersonWithIgnore() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonWithIgnore() { Age = 21 });
            yield return new TestCaseData(new PersonWithIgnore() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonWithoutContractWithDataMember());
            yield return new TestCaseData(new PersonWithoutContractWithDataMember() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonWithoutContractWithDataMember() { Age = 21 });
            yield return new TestCaseData(new PersonWithoutContractWithDataMember() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonGetSet());
            yield return new TestCaseData(new PersonGetSet() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonGetSet() { Age = 21 });
            yield return new TestCaseData(new PersonGetSet() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonContract());
            yield return new TestCaseData(new PersonContract() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonContract() { Age = 21 });
            yield return new TestCaseData(new PersonContract() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonContractMemberGetter());
            yield return new TestCaseData(new PersonContractMemberGetter() { FullName = "John Doe" });
            
            yield return new TestCaseData(new PersonContractWithNonPublicMember());
            yield return new TestCaseData(new PersonContractWithNonPublicMember() { FullName = "John Doe" });

            yield return new TestCaseData(new PersonContractOverrideName());
            yield return new TestCaseData(new PersonContractOverrideName() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonContractOverrideName() { Age = 21 });
            yield return new TestCaseData(new PersonContractOverrideName() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonContractOrdered());
            yield return new TestCaseData(new PersonContractOrdered() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonContractOrdered() { Age = 21 });
            yield return new TestCaseData(new PersonContractOrdered() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonWithDictionary());
            yield return new TestCaseData(new PersonWithDictionary() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonWithDictionary() { Age = 21 });
            yield return new TestCaseData(new PersonWithDictionary() { FullName = "John Doe", Age = 21 });
            yield return new TestCaseData(new PersonWithDictionary() { FullName = "John Doe", Age = 21, Dict = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } } });

            yield return new TestCaseData(new PersonContractWithIgnore());
            yield return new TestCaseData(new PersonContractWithIgnore() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonContractWithIgnore() { Age = 21 });
            yield return new TestCaseData(new PersonContractWithIgnore() { FullName = "John Doe", Age = 21 });

            yield return new TestCaseData(new PersonContractWithStruct());
            yield return new TestCaseData(new PersonContractWithStruct() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonContractWithStruct() { Age = 21 });
            yield return new TestCaseData(new PersonContractWithStruct() { FullName = "John Doe", Age = 21, LastLogin = DateTime.UtcNow });
            yield return new TestCaseData(new PersonContractWithStruct() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now });

            yield return new TestCaseData(new PersonContractWithNullable());
            yield return new TestCaseData(new PersonContractWithNullable() { FullName = "John Doe" });
            yield return new TestCaseData(new PersonContractWithNullable() { Age = 21 });
            yield return new TestCaseData(new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.UtcNow });
            yield return new TestCaseData(new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now });
        }

        [TestCaseSource(nameof(TestCases))]
        public void Tests<T>(T obj)
        {
            var options = new System.Text.Json.JsonSerializerOptions()
            {
                TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
            };

            // Serialize
            string json = System.Text.Json.JsonSerializer.Serialize(obj, options);
            string jsonExpected = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            Assert.That(json, Is.EqualTo(jsonExpected));
        }
    }
}