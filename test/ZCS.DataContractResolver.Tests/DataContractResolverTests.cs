using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xunit;

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

    public class PersonWithoutDefaultConstructor(string fullName, int age)
    {
        public string FullName { get; } = fullName;
        public int Age { get; } = age;
    }

    public class PersonWithNonPublicMember
    {
        public string FullName;
        protected int Age = 21;
    }

    public class PersonWithoutContractWithDataMember
    {
        [DataMember(Name = "full_name")]
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
    public class GenericWithConstructor<T>(T value)
    {
        [DataMember]
        public T Value { get; private set; } = value;
    }

    public class DataContractResolverTests
    {
        public static TheoryData<JsonIgnoreCondition, object> TestCases()
        {
            var data = new TheoryData<JsonIgnoreCondition, object>();

            var ignoreConditions = new List<JsonIgnoreCondition>()
            {
                JsonIgnoreCondition.Never,
                JsonIgnoreCondition.WhenWritingDefault,
                JsonIgnoreCondition.WhenWritingNull
            };

            foreach (var ignoreCondition in ignoreConditions)
            {
                data.Add(ignoreCondition, false);
                data.Add(ignoreCondition, true);

                data.Add(ignoreCondition, 'a');
                data.Add(ignoreCondition, (byte)1);
                data.Add(ignoreCondition, (sbyte)-1);

                data.Add(ignoreCondition, short.MinValue);
                data.Add(ignoreCondition, short.MaxValue);

                data.Add(ignoreCondition, int.MinValue);
                data.Add(ignoreCondition, int.MaxValue);

                data.Add(ignoreCondition, long.MinValue);
                data.Add(ignoreCondition, Int64.MaxValue);

                data.Add(ignoreCondition, ushort.MinValue);
                data.Add(ignoreCondition, ushort.MaxValue);

                data.Add(ignoreCondition, uint.MinValue);
                data.Add(ignoreCondition, uint.MaxValue);

                data.Add(ignoreCondition, ulong.MinValue);
                data.Add(ignoreCondition, ulong.MaxValue);

                data.Add(ignoreCondition, 1.2m);

                data.Add(ignoreCondition, string.Empty);
                data.Add(ignoreCondition, "Hello");

                data.Add(ignoreCondition, default(DateTime));
                data.Add(ignoreCondition, DateTime.Now);

                data.Add(ignoreCondition, default(TimeSpan));
                data.Add(ignoreCondition, TimeSpan.FromSeconds(1234567890));

                data.Add(ignoreCondition, (1, 2));
                data.Add(ignoreCondition, (1, "2"));
                data.Add(ignoreCondition, ("1", "2"));

                data.Add(ignoreCondition, new KeyValuePair<int, int>(1, 2));
                data.Add(ignoreCondition, new KeyValuePair<int, string>(1, "2"));
                data.Add(ignoreCondition, new KeyValuePair<string, string>("1", "2"));

                data.Add(ignoreCondition, new List<int>());
                data.Add(ignoreCondition, (List<int>)[0, 1, 2, 3]);
                data.Add(ignoreCondition, (List<string>)["0", "1", "2", "3"]);

                data.Add(ignoreCondition, Array.Empty<int>());
                data.Add(ignoreCondition, (int[])[0, 1, 2, 3]);
                data.Add(ignoreCondition, (string[])["0", "1", "2", "3"]);

                data.Add(ignoreCondition, new Dictionary<int, int> { { 1, 2 }, { 3, 4 } });
                data.Add(ignoreCondition, new Dictionary<int, string> { { 1, "2" }, { 3, "4" } });
                data.Add(ignoreCondition, new Dictionary<string, string> { { "1", "2" }, { "3", "4" } });

                data.Add(ignoreCondition, new HashSet<int>());
                data.Add(ignoreCondition, (HashSet<int>)[0, 1, 2, 3]);
                data.Add(ignoreCondition, (HashSet<string>)["0", "1", "2", "3"]);

                data.Add(ignoreCondition, new Generic<int>{ Value = 1 });
                data.Add(ignoreCondition, new Generic<string> { Value = "1" });

                data.Add(ignoreCondition, new GenericWithConstructor<int>(1));
                data.Add(ignoreCondition, new GenericWithConstructor<string>("1"));

                data.Add(ignoreCondition, PersonEnum.First);
                data.Add(ignoreCondition, PersonEnum.Second);
                data.Add(ignoreCondition, PersonEnum.Third);
                data.Add(ignoreCondition, PersonEnum.Fourth);

                data.Add(ignoreCondition, new Person());
                data.Add(ignoreCondition, new Person() { FullName = "John Doe" });
                data.Add(ignoreCondition, new Person() { Age = 21 });
                data.Add(ignoreCondition, new Person() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonWithoutDefaultConstructor("John Doe", 21));

                data.Add(ignoreCondition, new PersonWithNonPublicMember());
                data.Add(ignoreCondition, new PersonWithNonPublicMember() { FullName = "John Doe" });

                data.Add(ignoreCondition, new PersonContractWithoutDataMember());
                data.Add(ignoreCondition, new PersonContractWithoutDataMember() { FullName = "John Doe" });

                data.Add(ignoreCondition, new PersonWithIgnore());
                data.Add(ignoreCondition, new PersonWithIgnore() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonWithIgnore() { Age = 21 });
                data.Add(ignoreCondition, new PersonWithIgnore() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonWithoutContractWithDataMember());
                data.Add(ignoreCondition, new PersonWithoutContractWithDataMember() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonWithoutContractWithDataMember() { Age = 21 });
                data.Add(ignoreCondition, new PersonWithoutContractWithDataMember() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonGetSet());
                data.Add(ignoreCondition, new PersonGetSet() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonGetSet() { Age = 21 });
                data.Add(ignoreCondition, new PersonGetSet() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonContract());
                data.Add(ignoreCondition, new PersonContract() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContract() { Age = 21 });
                data.Add(ignoreCondition, new PersonContract() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonContractMemberGetter());
                data.Add(ignoreCondition, new PersonContractMemberGetter() { FullName = "John Doe" });

                data.Add(ignoreCondition, new PersonContractWithNonPublicMember());
                data.Add(ignoreCondition, new PersonContractWithNonPublicMember() { FullName = "John Doe" });

                data.Add(ignoreCondition, new PersonContractOverrideName());
                data.Add(ignoreCondition, new PersonContractOverrideName() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContractOverrideName() { Age = 21 });
                data.Add(ignoreCondition, new PersonContractOverrideName() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonContractOrdered());
                data.Add(ignoreCondition, new PersonContractOrdered() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContractOrdered() { Age = 21 });
                data.Add(ignoreCondition, new PersonContractOrdered() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonWithDictionary());
                data.Add(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonWithDictionary() { Age = 21 });
                data.Add(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe", Age = 21 });
                data.Add(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe", Age = 21, Friends = new Dictionary<int, Person> { { 1, new Person() { FullName = "John Doe", Age = 21 } }, { 2, new Person() { FullName = "James Doe", Age = 22 } } } });
                data.Add(ignoreCondition, new PersonWithDictionary() { FullName = "John Doe", Age = 21, Dict = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } } });

                data.Add(ignoreCondition, new PersonWithList());
                data.Add(ignoreCondition, new PersonWithList() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonWithList() { Age = 21 });
                data.Add(ignoreCondition, new PersonWithList() { FullName = "John Doe", Age = 21 });
                data.Add(ignoreCondition, new PersonWithList() { FullName = "John Doe", Age = 21, Friends = [new() { FullName = "John Doe", Age = 21 }] });
                data.Add(ignoreCondition, new PersonWithList() { FullName = "John Doe", Age = 21, List = [0, 1, 2, 3, 4] });

                data.Add(ignoreCondition, new PersonWithSet());
                data.Add(ignoreCondition, new PersonWithSet() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonWithSet() { Age = 21 });
                data.Add(ignoreCondition, new PersonWithSet() { FullName = "John Doe", Age = 21 });
                data.Add(ignoreCondition, new PersonWithSet() { FullName = "John Doe", Age = 21, Friends = [new() { FullName = "John Doe", Age = 21 }, new() { FullName = "James Doe", Age = 22 }] });
                data.Add(ignoreCondition, new PersonWithSet() { FullName = "John Doe", Age = 21, Set = [0, 1, 2, 3, 4] });

                data.Add(ignoreCondition, new PersonWithArray());
                data.Add(ignoreCondition, new PersonWithArray() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonWithArray() { Age = 21 });
                data.Add(ignoreCondition, new PersonWithArray() { FullName = "John Doe", Age = 21 });
                data.Add(ignoreCondition, new PersonWithArray() { FullName = "John Doe", Age = 21, Friends = [new() { FullName = "John Doe", Age = 21 }, new() { FullName = "James Doe", Age = 22 }] });
                data.Add(ignoreCondition, new PersonWithArray() { FullName = "John Doe", Age = 21, Array = [0, 1, 2, 3, 4] });

                data.Add(ignoreCondition, new PersonContractWithIgnore());
                data.Add(ignoreCondition, new PersonContractWithIgnore() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContractWithIgnore() { Age = 21 });
                data.Add(ignoreCondition, new PersonContractWithIgnore() { FullName = "John Doe", Age = 21 });

                data.Add(ignoreCondition, new PersonContractWithStruct());
                data.Add(ignoreCondition, new PersonContractWithStruct() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContractWithStruct() { Age = 21 });
                data.Add(ignoreCondition, new PersonContractWithStruct() { FullName = "John Doe", Age = 21, LastLogin = DateTime.UtcNow });
                data.Add(ignoreCondition, new PersonContractWithStruct() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now });

                data.Add(ignoreCondition, new PersonContractWithNullable());
                data.Add(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContractWithNullable() { Age = 21 });
                data.Add(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.UtcNow });
                data.Add(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now });
                data.Add(ignoreCondition, new PersonContractWithNullable() { FullName = "John Doe", Age = 21, LastLogin = DateTime.Now, BestFriend = new PersonWithoutDefaultConstructor("James Doe", 20)});

                data.Add(ignoreCondition, new PersonContractWithEnum());
                data.Add(ignoreCondition, new PersonContractWithEnum() { FullName = "John Doe" });
                data.Add(ignoreCondition, new PersonContractWithEnum() { Age = 21 });
                data.Add(ignoreCondition, new PersonContractWithEnum() { FullName = "John Doe", Age = 21, Enum = PersonEnum.Second });
                data.Add(ignoreCondition, new PersonContractWithEnum() { FullName = "John Doe", Age = 21, Enum = PersonEnum.Second, Dict = new Dictionary<PersonEnum, string>() { { PersonEnum.Third, "3"}, { PersonEnum.Fourth, "4" } } });

                data.Add(ignoreCondition, new PersonContractWithBasicTypes());
                data.Add(ignoreCondition, new PersonContractWithBasicTypes() { Byte = 1, SByte = -1, Short = -2, UShort = 2, Int = -3, UInt = 3, Long = -4, ULong = 4, Float = 1.2f, Double = 2.345, Decimal = 12.34m, Char = 'c' });
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Tests<T>(JsonIgnoreCondition ignoreCondition, T obj)
        {
            var options = new System.Text.Json.JsonSerializerOptions()
            {
                TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
                DefaultIgnoreCondition = ignoreCondition
            };

            var newtonsoftSettings = new JsonSerializerSettings()
            {
                DefaultValueHandling = ignoreCondition == JsonIgnoreCondition.WhenWritingDefault ? DefaultValueHandling.Ignore : DefaultValueHandling.Include,
                NullValueHandling = ignoreCondition == JsonIgnoreCondition.WhenWritingNull ? NullValueHandling.Ignore : NullValueHandling.Include
            };

            // Serialize
            string json = System.Text.Json.JsonSerializer.Serialize(obj, options);
            string jsonExpected = JsonConvert.SerializeObject(obj, newtonsoftSettings);

            Assert.Equal(json, jsonExpected);

            // Deserialize
            T obj2 = System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
            T obj3 = JsonConvert.DeserializeObject<T>(json, newtonsoftSettings);

            string jsonExpected2 = JsonConvert.SerializeObject(obj2, newtonsoftSettings);
            string jsonExpected3 = JsonConvert.SerializeObject(obj3, newtonsoftSettings);

            Assert.Equal(json, jsonExpected2);
            Assert.Equal(json, jsonExpected3);
        }
    }
}