using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ZCS.DataContractResolver.Tests
{
    public enum PersonEnum
    {
        First,
        Second,
        Third,
        Fourth
    }

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

        [DataMember(EmitDefaultValue = false)]
        public PersonWithoutDefaultConstructor BestFriend { get; set; }
    }

    [DataContract]
    public class PersonContractWithEnum
    {
        [DataMember(EmitDefaultValue = false)]
        public string FullName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Age { get; set; }

        [DataMember]
        public PersonEnum Enum { get; set; }

        [DataMember]
        public Dictionary<PersonEnum, string> Dict { get; set; }
    }

    [DataContract]
    public class PersonContractWithBasicTypes
    {
        public byte Byte { get; set; }
        
        public sbyte SByte { get; set; }

        public short Short { get; set; }

        public ushort UShort { get; set; }

        public int Int { get; set; }

        public uint UInt { get; set; }

        public long Long { get; set; }

        public ulong ULong { get; set; }

        public float Float { get; set; }

        public double Double { get; set; }

        public decimal Decimal { get; set; }

        public char Char { get; set; }
    }

    [DataContract]
    public class Generic<T>
    {
        [DataMember]
        public T Value { get; set; }
    }

    [DataContract]
    public class GenericWithConstructor<T>
    {
        public GenericWithConstructor(T value)
        {
            Value = value;
        }

        [DataMember]
        public T Value { get; private set; }
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
                yield return new TestCaseData(ignoreCondition, false);
                yield return new TestCaseData(ignoreCondition, true);

                yield return new TestCaseData(ignoreCondition, 'a');
                yield return new TestCaseData(ignoreCondition, (byte)1);
                yield return new TestCaseData(ignoreCondition, (sbyte)-1);

                yield return new TestCaseData(ignoreCondition, Int16.MinValue);
                yield return new TestCaseData(ignoreCondition, Int16.MaxValue);

                yield return new TestCaseData(ignoreCondition, Int32.MinValue);
                yield return new TestCaseData(ignoreCondition, Int32.MaxValue);

                yield return new TestCaseData(ignoreCondition, Int64.MinValue);
                yield return new TestCaseData(ignoreCondition, Int64.MaxValue);

                yield return new TestCaseData(ignoreCondition, UInt16.MinValue);
                yield return new TestCaseData(ignoreCondition, UInt16.MaxValue);

                yield return new TestCaseData(ignoreCondition, UInt32.MinValue);
                yield return new TestCaseData(ignoreCondition, UInt32.MaxValue);

                yield return new TestCaseData(ignoreCondition, UInt64.MinValue);
                yield return new TestCaseData(ignoreCondition, UInt64.MaxValue);

                yield return new TestCaseData(ignoreCondition, 1.2m);

                yield return new TestCaseData(ignoreCondition, string.Empty);
                yield return new TestCaseData(ignoreCondition, "Hello");

                yield return new TestCaseData(ignoreCondition, default(DateTime));
                yield return new TestCaseData(ignoreCondition, DateTime.Now);

                yield return new TestCaseData(ignoreCondition, default(TimeSpan));
                yield return new TestCaseData(ignoreCondition, TimeSpan.FromSeconds(1234567890));

                yield return new TestCaseData(ignoreCondition, (1, 2));
                yield return new TestCaseData(ignoreCondition, (1, "2"));
                yield return new TestCaseData(ignoreCondition, ("1", "2"));

                yield return new TestCaseData(ignoreCondition, new KeyValuePair<int, int>(1, 2));
                yield return new TestCaseData(ignoreCondition, new KeyValuePair<int, string>(1, "2"));
                yield return new TestCaseData(ignoreCondition, new KeyValuePair<string, string>("1", "2"));

                yield return new TestCaseData(ignoreCondition, new List<int>());
                yield return new TestCaseData(ignoreCondition, new List<int> { 0, 1, 2, 3 });
                yield return new TestCaseData(ignoreCondition, new List<string> { "0", "1", "2", "3" });

                yield return new TestCaseData(ignoreCondition, new int[0]);
                yield return new TestCaseData(ignoreCondition, new int[] { 0, 1, 2, 3 });
                yield return new TestCaseData(ignoreCondition, new string[] { "0", "1", "2", "3" });

                yield return new TestCaseData(ignoreCondition, new Dictionary<int, int> { { 1, 2 }, { 3, 4 } });
                yield return new TestCaseData(ignoreCondition, new Dictionary<int, string> { { 1, "2" }, { 3, "4" } });
                yield return new TestCaseData(ignoreCondition, new Dictionary<string, string> { { "1", "2" }, { "3", "4" } });

                yield return new TestCaseData(ignoreCondition, new HashSet<int>());
                yield return new TestCaseData(ignoreCondition, new HashSet<int> { 0, 1, 2, 3 });
                yield return new TestCaseData(ignoreCondition, new HashSet<string> { "0", "1", "2", "3" });

                yield return new TestCaseData(ignoreCondition, new Generic<int>{ Value = 1 });
                yield return new TestCaseData(ignoreCondition, new Generic<string> { Value = "1" });

                yield return new TestCaseData(ignoreCondition, new GenericWithConstructor<int>(1));
                yield return new TestCaseData(ignoreCondition, new GenericWithConstructor<string>("1"));

                yield return new TestCaseData(ignoreCondition, PersonEnum.First);
                yield return new TestCaseData(ignoreCondition, PersonEnum.Second);
                yield return new TestCaseData(ignoreCondition, PersonEnum.Third);
                yield return new TestCaseData(ignoreCondition, PersonEnum.Fourth);

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
                yield return new TestCaseData(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now, BestFriend = new PersonWithoutDefaultConstructor("James Doe", 20)});

                yield return new TestCaseData(ignoreCondition, new PersonContractWithEnum());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithEnum() { FullName = "John Doe" });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithEnum() { Age = 21 });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithEnum() { FullName = "John Doe", Age = 21, Enum = PersonEnum.Second });
                yield return new TestCaseData(ignoreCondition, new PersonContractWithEnum() { FullName = "John Doe", Age = 21, Enum = PersonEnum.Second, Dict = new Dictionary<PersonEnum, string>() { { PersonEnum.Third, "3"}, { PersonEnum.Fourth, "4" } } });

                yield return new TestCaseData(ignoreCondition, new PersonContractWithBasicTypes());
                yield return new TestCaseData(ignoreCondition, new PersonContractWithBasicTypes() { Byte = 1, SByte = -1, Short = -2, UShort = 2, Int = -3, UInt = 3, Long = -4, ULong = 4, Float = 1.2f, Double = 2.345, Decimal = 12.34m, Char = 'c' });
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
            T obj3 = JsonConvert.DeserializeObject<T>(json, newtonsoftSettings);
            
            string jsonExpected2 = JsonConvert.SerializeObject(obj2, newtonsoftSettings);
            string jsonExpected3 = JsonConvert.SerializeObject(obj3, newtonsoftSettings);

            Assert.That(json, Is.EqualTo(jsonExpected2));
            Assert.That(json, Is.EqualTo(jsonExpected3));
        }
    }
}