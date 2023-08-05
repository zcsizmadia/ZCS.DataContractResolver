using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ZCS.DataContractResolver.Tests
{
    public class Person
    {
        public string FullName;
        public int Age;
    }

    public class PersonWithoutDefaultConstructor
    {
        public PersonWithoutDefaultConstructor(string fullName, int age)
        {
            FullName = fullName;
            Age = age;
        }
        public string FullName { get; }
        public int Age { get; }
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
        public Dictionary<int, Person> Friends { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, string> Dict { get; set; }
    }

    [DataContract]
    public class PersonWithList
    {
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string FullName { get; set; }

        [DataMember(Order = 1)]
        public int Age { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Person> Friends { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<int> List { get; set; }
    }

    [DataContract]
    public class PersonWithSet
    {
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string FullName { get; set; }

        [DataMember(Order = 1)]
        public int Age { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public HashSet<Person> Friends { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public HashSet<int> Set { get; set; }
    }

    [DataContract]
    public class PersonWithArray
    {
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string FullName { get; set; }

        [DataMember(Order = 1)]
        public int Age { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Person[] Friends { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int[] Array { get; set; }
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
            var ignoreConditions = new List<System.Text.Json.Serialization.JsonIgnoreCondition>()
            {
                System.Text.Json.Serialization.JsonIgnoreCondition.Never,
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            foreach (var ignoreCondition in ignoreConditions)
            {
                yield return new TestCaseData(ignoreCondition, new Person());
                yield return new TestCaseData(ignoreCondition, new Person() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new Person() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new Person() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonWithoutDefaultConstructor("John Doe", 21));

                yield return new TestCaseData(ignoreCondition, new PersonWithNonPublicMember());
                yield return new TestCaseData(ignoreCondition, new PersonWithNonPublicMember() { FullName = "John Doe" });

                yield return new TestCaseData(ignoreCondition, new PersonContractWithoutDataMember());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithoutDataMember() { FullName = "John Doe" });

                yield return new TestCaseData(ignoreCondition, new PersonWithIgnore());
                yield return new TestCaseData(ignoreCondition, new PersonWithIgnore() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonWithIgnore() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithIgnore() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonWithoutContractWithDataMember());
                yield return new TestCaseData(ignoreCondition, new PersonWithoutContractWithDataMember() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonWithoutContractWithDataMember() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithoutContractWithDataMember() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonGetSet());
                yield return new TestCaseData(ignoreCondition, new PersonGetSet() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonGetSet() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonGetSet() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonContract());
                yield return new TestCaseData(ignoreCondition, new PersonContract() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContract() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContract() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonContractMemberGetter());
                yield return new TestCaseData(ignoreCondition, new PersonContractMemberGetter() { FullName = "John Doe" });

                yield return new TestCaseData(ignoreCondition, new PersonContractWithNonPublicMember());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithNonPublicMember() { FullName = "John Doe" });

                yield return new TestCaseData(ignoreCondition, new PersonContractOverrideName());
                yield return new TestCaseData(ignoreCondition, new PersonContractOverrideName() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContractOverrideName() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContractOverrideName() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonContractOrdered());
                yield return new TestCaseData(ignoreCondition, new PersonContractOrdered() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContractOrdered() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContractOrdered() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonWithDictionary());
                yield return new TestCaseData(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonWithDictionary() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe", Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe", Age = 21, Friends = new Dictionary<int, Person> { { 1, new Person() { FullName = "John Doe", Age = 21 } }, { 2, new Person() { FullName = "James Doe", Age = 22 } } } });
                yield return new TestCaseData(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe", Age = 21, Dict = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } } });

                yield return new TestCaseData(ignoreCondition, new PersonWithList());
                yield return new TestCaseData(ignoreCondition, new PersonWithList() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonWithList() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithList() { FullName = "John Doe", Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithList() { FullName = "John Doe", Age = 21, Friends = new List<Person> { new Person() { FullName = "John Doe", Age = 21 } } });
                yield return new TestCaseData(ignoreCondition, new PersonWithList() { FullName = "John Doe", Age = 21, List = new List<int> { 0, 1, 2, 3, 4 } });

                yield return new TestCaseData(ignoreCondition, new PersonWithSet());
                yield return new TestCaseData(ignoreCondition, new PersonWithSet() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonWithSet() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithSet() { FullName = "John Doe", Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithSet() { FullName = "John Doe", Age = 21, Friends = new HashSet<Person> { new Person() { FullName = "John Doe", Age = 21 }, new Person() { FullName = "James Doe", Age = 22 } } });
                yield return new TestCaseData(ignoreCondition, new PersonWithSet() { FullName = "John Doe", Age = 21, Set = new HashSet<int> { 0, 1, 2, 3, 4 } });

                yield return new TestCaseData(ignoreCondition, new PersonWithArray());
                yield return new TestCaseData(ignoreCondition, new PersonWithArray() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonWithArray() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithArray() { FullName = "John Doe", Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonWithArray() { FullName = "John Doe", Age = 21, Friends = new Person[] { new Person() { FullName = "John Doe", Age = 21 }, new Person() { FullName = "James Doe", Age = 22 } } });
                yield return new TestCaseData(ignoreCondition, new PersonWithArray() { FullName = "John Doe", Age = 21, Array = new int[] { 0, 1, 2, 3, 4 } });

                yield return new TestCaseData(ignoreCondition, new PersonContractWithIgnore());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithIgnore() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithIgnore() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithIgnore() { FullName = "John Doe", Age = 21 });

                yield return new TestCaseData(ignoreCondition, new PersonContractWithStruct());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithStruct() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithStruct() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithStruct() { FullName = "John Doe", Age = 21, LastLogin = DateTime.UtcNow });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithStruct() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now });

                yield return new TestCaseData(ignoreCondition, new PersonContractWithNullable());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithNullable() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.UtcNow });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now });
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void Tests<T>(System.Text.Json.Serialization.JsonIgnoreCondition ignoreCondition, T obj)
        {
            var options = new System.Text.Json.JsonSerializerOptions()
            {
                TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
                DefaultIgnoreCondition = ignoreCondition
            };

            var newtonsoftSettings = new JsonSerializerSettings()
            {
                DefaultValueHandling = ignoreCondition == System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault ? DefaultValueHandling.Ignore : DefaultValueHandling.Include,
                NullValueHandling = ignoreCondition == System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull ? NullValueHandling.Ignore : NullValueHandling.Include
            };

            // Serialize
            string json = System.Text.Json.JsonSerializer.Serialize(obj, options);
            string jsonExpected = JsonConvert.SerializeObject(obj, newtonsoftSettings);

            Assert.That(json, Is.EqualTo(jsonExpected));

            // Deserialize
            T obj2 = System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
            string jsonExpected2 = JsonConvert.SerializeObject(obj2, newtonsoftSettings);

            Assert.That(json, Is.EqualTo(jsonExpected2));
        }
    }
}