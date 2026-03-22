namespace ZCS.DataContractResolver.Tests;

using Newtonsoft.Json.Tests.TestObjects;
using Newtonsoft.Json.Tests.TestObjects.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using Xunit;

public class ContractResolverTests
{
    internal class SerializableTestObject : ISerializableTestObject
    {
        public SerializableTestObject(string stringValue, int intValue, DateTimeOffset dateTimeOffset, Newtonsoft.Json.Tests.TestObjects.Organization.Person personValue) : base(stringValue, intValue, dateTimeOffset, personValue)
        {
        }

        protected SerializableTestObject(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    internal class SerializableWithoutAttributeTestObject : ISerializableWithoutAttributeTestObject;

    public class CustomList<T> : List<T>;

    public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>;

    public class AddressWithDataMember
    {
        [DataMember(Name = "CustomerAddress1")]
        public string AddressLine1 { get; set; }
    }

    public class Book
    {
        public string BookName { get; set; }
        public decimal BookPrice { get; set; }
        public string AuthorName { get; set; }
        public int AuthorAge { get; set; }
        public string AuthorCountry { get; set; }
    }

    [Fact]
    public void SerializeSerializableTestObject()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var testObject = new SerializableTestObject("stringValue", 123, DateTimeOffset.Now, new Newtonsoft.Json.Tests.TestObjects.Organization.Person());
        var json = JsonSerializer.Serialize(testObject, options);

        Assert.NotNull(json);
    }

    [Fact]
    public void SerializeSerializableWithoutAttributeTestObject()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var testObject = new SerializableWithoutAttributeTestObject();
        var json = JsonSerializer.Serialize(testObject, options);

        Assert.NotNull(json);
    }

    [Fact]
    public void SerializeAnswerFilterModel()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var model = new AnswerFilterModel();
        var json = JsonSerializer.Serialize(model, options);

        Assert.NotNull(json);
    }

    [Fact]
    public void AbstractTestClass()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<AbstractTestClass>(@"{""Value"":""Value!""}", options));

        var o = JsonSerializer.Deserialize<AbstractImplementationTestClass>(@"{""Value"":""Value!""}", options);

        Assert.Equal("Value!", o.Value);
    }

    [Fact]
    public void AbstractListTestClass()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<AbstractListTestClass<int>>("[1,2]", options));

        var l = JsonSerializer.Deserialize<AbstractImplementationListTestClass<int>>("[1,2]", options);

        Assert.Equal(2, l.Count);
        Assert.Equal(1, l[0]);
        Assert.Equal(2, l[1]);
    }

    [Fact]
    public void ListInterfaceDefaultCreator()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var l = JsonSerializer.Deserialize<CustomList<int>>("[1,2,3]", options);

        Assert.Equal(typeof(CustomList<int>), l.GetType());
        Assert.Equal(3, l.Count);
        Assert.Equal(1, l[0]);
        Assert.Equal(2, l[1]);
        Assert.Equal(3, l[2]);
    }

    [Fact]
    public void DictionaryInterfaceDefaultCreator()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var d = JsonSerializer.Deserialize<CustomDictionary<string, int>>(@"{""key1"":1,""key2"":2}", options);

        Assert.Equal(typeof(CustomDictionary<string, int>), d.GetType());
        Assert.Equal(2, d.Count);
        Assert.Equal(1, d["key1"]);
        Assert.Equal(2, d["key2"]);
    }

    [Fact]
    public void AbstractDictionaryTestClass()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<AbstractDictionaryTestClass<string, int>>(@"{""key1"":1,""key2"":2}", options));

        var d = JsonSerializer.Deserialize<AbstractImplementationDictionaryTestClass<string, int>>(@"{""key1"":1,""key2"":2}", options);

        Assert.Equal(2, d.Count);
        Assert.Equal(1, d["key1"]);
        Assert.Equal(2, d["key2"]);
    }

    [Fact]
    public void SerializeWithEscapedPropertyName()
    {
        var options = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(
            new AddressWithDataMember
            {
                AddressLine1 = "value!"
            }, options);

        Assert.Equal(@"{""AddressLine1"":""value!""}", json);
    }

    [Fact(Skip = "HTML escaping is not supported in System.Text.Json yet.")]
    public void SerializeWithHtmlEscapedPropertyName()
    {
        var options = new JsonSerializerOptions()
        {
            // Encoder = new SomeEncoderHere(),
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var address = new AddressWithDataMember { AddressLine1 = "value!" };
        string json = JsonSerializer.Serialize(address, options);

        Assert.Contains("\"\\u003cb\\u003eAddressLine1\\u003c/b\\u003e\":\"value!\"", json);

        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        JsonProperty property = root.EnumerateObject().First();

        Assert.Equal("<b>AddressLine1</b>", property.Name);
    }

    [Fact]
    public void SerializeWithSpecialCharactersInPropertyName()
    {
        var options = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(
            new Dictionary<string, string>
            {
                { "abc", "value1" },
                { "123", "value2" },
                { "._-", "value3" },
                { "!@#", "value4" },
                { "$%^", "value5" },
                { "?*(", "value6" },
                { ")_+", "value7" },
                { "=:,", "value8" },
                { "&", "value10" },
                { "<", "value11" },
                { ">", "value12" },
                { "'", "value13" },
                { "\"", "value14" },
                { "\r\n", "value15" },
                { "\0", "value16" },
                { "\n", "value17" },
                { "\v", "value18" },
                { "\u00B9", "value19" },
            }, options);

        Assert.NotNull(json);
    }

    [Fact]
    public void SerializeWithDataMemberClassWithDataContract()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new AddressWithDataMember(), options);

        Assert.Contains("AddressLine1", json);
    }

    public class PublicParameterizedConstructorWithPropertyNameConflictWithAttribute
    {
        public PublicParameterizedConstructorWithPropertyNameConflictWithAttribute()
        {
        }

        [JsonPropertyName("name")]
        public string NameParameter { get; set; }

        public int Name
        {
            get { return Convert.ToInt32(NameParameter); }
        }
    }

    [Fact]
    public void SerializeWithParameterizedCreator()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new PublicParameterizedConstructorWithPropertyNameConflictWithAttribute(), options);

        Assert.Contains("name", json, StringComparison.InvariantCultureIgnoreCase);
    }

    [Fact]
    public void SerializeWithCustomOverrideCreator()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new MultipleParametrizedConstructorsJsonConstructor("value!", 1), options);

        Assert.Contains("value!", json);
        Assert.Contains("1", json);
    }

    [Fact]
    public void SerializeInterface()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        Employee employee = new()
        {
            BirthDate = new DateTime(1977, 12, 30, 1, 1, 1, DateTimeKind.Utc),
            FirstName = "Maurice",
            LastName = "Moss",
            Department = "IT",
            JobTitle = "Support"
        };

        string iPersonJson = JsonSerializer.Serialize(employee, options);

        Assert.Contains("Maurice", iPersonJson);
        Assert.Contains("Moss", iPersonJson);
        Assert.Contains("1977-12-30T01:01:01", iPersonJson);
    }

        [Fact]
    public void SingleTypeWithMultipleContractResolvers()
    {
        Book book = new()
        {
            BookName = "The Gathering Storm",
            BookPrice = 16.19m,
            AuthorName = "Brandon Sanderson",
            AuthorAge = 34,
            AuthorCountry = "United States of America"
        };

        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        string json = JsonSerializer.Serialize(book, options);

        Assert.Contains("The Gathering Storm", json);
        Assert.Contains("16.19", json);
        Assert.Contains("Brandon Sanderson", json);
        Assert.Contains("34", json);
        Assert.Contains("United States of America", json);
    }

    [Fact]
    public void SerializeCompilerGeneratedMembers()
    {
        StructTest structTest = new()
        {
            IntField = 1,
            IntProperty = 2,
            StringField = "Field",
            StringProperty = "Property"
        };

        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        string json = JsonSerializer.Serialize(structTest, options);

        Assert.Contains("Field", json);
        Assert.Contains("1", json);
        Assert.Contains("Property", json);
        Assert.Contains("2", json);
    }

    public class ClassWithExtensionData
    {
        [JsonExtensionData]
        public IDictionary<string, JsonElement> Data { get; set; }
    }

    [Fact]
    public void ExtensionDataGetterCanBeIteratedMultipleTimes()
    {
        ClassWithExtensionData myClass = new()
        {
            Data = new Dictionary<string, JsonElement>
            {
                { "SomeField", JsonDocument.Parse("\"Field\"").RootElement },
            }
        };

        Assert.True(myClass.Data.Any());
        Assert.True(myClass.Data.Any());
    }

    public class ClassWithShouldSerialize
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }

    [Fact]
    public void SerializeClassWithShouldSerialize()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new ClassWithShouldSerialize { Prop1 = "Value1", Prop2 = "Value2" }, options);

        Assert.Contains("Prop1", json);
        Assert.Contains("Prop2", json);
    }

    public class ClassWithIsSpecified
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
        public string Prop4 { get; set; }
        public string Prop5 { get; set; }

        public bool Prop1Specified;
        public bool Prop2Specified { get; set; }
        public static bool Prop3Specified { get; set; }
        public event Func<bool> Prop4Specified;
        public static bool Prop5Specified;

        protected virtual bool OnProp4Specified()
        {
            return Prop4Specified?.Invoke() ?? false;
        }
    }

    [Fact(Skip ="IgnoreIsSpecifiedMembers is not supported yet.")]
    public void SerializeClassWithIsSpecified()
    {
        // DefaultContractResolver resolver = new DefaultContractResolver();
        // resolver.IgnoreIsSpecifiedMembers = true;

        // JsonObjectContract contract = (JsonObjectContract)resolver.ResolveContract(typeof(ClassWithIsSpecified));

        // var property1 = contract.Properties["Prop1"];
        // Assert.AreEqual(null, property1.GetIsSpecified);
        // Assert.AreEqual(null, property1.SetIsSpecified);

        // var property2 = contract.Properties["Prop2"];
        // Assert.AreEqual(null, property2.GetIsSpecified);
        // Assert.AreEqual(null, property2.SetIsSpecified);

        // var property3 = contract.Properties["Prop3"];
        // Assert.AreEqual(null, property3.GetIsSpecified);
        // Assert.AreEqual(null, property3.SetIsSpecified);

        // var property4 = contract.Properties["Prop4"];
        // Assert.AreEqual(null, property4.GetIsSpecified);
        // Assert.AreEqual(null, property4.SetIsSpecified);

        // var property5 = contract.Properties["Prop5"];
        // Assert.AreEqual(null, property5.GetIsSpecified);
        // Assert.AreEqual(null, property5.SetIsSpecified);
    }

    public class RequiredPropertyTestClass
    {
        [Required]
        public string Name { get; set; }
    }

    [Fact]
    public void SerializeClassWithRequiredProperty()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new RequiredPropertyTestClass { Name = "Test" }, options);

        Assert.Contains("Name", json);
    }

    public class RequiredObject
    {
        public string UnsetProperty { get; set; }
        [Required]
        public string AllowNullProperty { get; set; }
    }

    [Fact]
    public void SerializeClassWithRequiredObject()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new RequiredObject { AllowNullProperty = "Test" }, options);

        Assert.Contains("AllowNullProperty", json);
    }

    [Fact]
    public void SerializeClassWithoutRequiredObject()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        const string json = "{}";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<RequiredObject>(json, options));
    }

    [Fact]
    public void SerializeObject()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new object(), options);

        Assert.Equal("{}", json);
    }

    [Fact]
    public void SerializeRegex()
    {
        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var regex = new Regex("pattern");

        Assert.Throws<NotSupportedException>(() =>
        {
            var json = JsonSerializer.Serialize(regex, options);
            return JsonSerializer.Deserialize<Regex>(json, options);
        });
    }

    [Fact]
    public void GetTypeInfo_NullJsonTypeInfo_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            System.Text.Json.Serialization.Metadata.DataContractResolver.GetTypeInfo(null!));
    }

    [Fact]
    public void GetTypeInfo_NullType_ThrowsArgumentNullException()
    {
        var resolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default;
        var options = new JsonSerializerOptions();

        Assert.Throws<ArgumentNullException>(() => resolver.GetTypeInfo(null!, options));
    }

    [Fact]
    public void GetTypeInfo_NullOptions_ThrowsArgumentNullException()
    {
        var resolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default;

        Assert.Throws<ArgumentNullException>(() => resolver.GetTypeInfo(typeof(object), null!));
    }

    [DataContract]
    public class StructWithEmitDefaultValue
    {
        [DataMember(EmitDefaultValue = false)]
        public int IntValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double DoubleValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool BoolValue { get; set; }
    }

    [Fact]
    public void SerializeStruct_EmitDefaultValue_False_OmitsDefaultValueTypes()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new StructWithEmitDefaultValue();
        var json = JsonSerializer.Serialize(obj, options);

        Assert.Equal("{}", json);
    }

    [Fact]
    public void SerializeStruct_EmitDefaultValue_False_IncludesNonDefaultValueTypes()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new StructWithEmitDefaultValue { IntValue = 42, DoubleValue = 3.14, BoolValue = true };
        var json = JsonSerializer.Serialize(obj, options);

        Assert.Contains("IntValue", json);
        Assert.Contains("DoubleValue", json);
        Assert.Contains("BoolValue", json);
    }

    [Fact]
    public void Default_ReturnsSameInstance()
    {
        var first = System.Text.Json.Serialization.Metadata.DataContractResolver.Default;
        var second = System.Text.Json.Serialization.Metadata.DataContractResolver.Default;

        Assert.Same(first, second);
    }

    // ---- Inheritance tests ----

    [DataContract]
    public class BasePersonContract
    {
        [DataMember(Name = "base_name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class DerivedPersonContract : BasePersonContract
    {
        [DataMember(Name = "dept")]
        public string Department { get; set; }
    }

    [Fact]
    public void SerializeInheritedDataContractClass_IncludesBaseClassDataMembers()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new DerivedPersonContract { Name = "John", Department = "IT" };
        var json = JsonSerializer.Serialize(obj, options);

        Assert.Contains("base_name", json);
        Assert.Contains("John", json);
        Assert.Contains("dept", json);
        Assert.Contains("IT", json);

        var deserialized = JsonSerializer.Deserialize<DerivedPersonContract>(json, options);
        Assert.Equal("John", deserialized.Name);
        Assert.Equal("IT", deserialized.Department);
    }

    public class DerivedPersonWithoutContract : BasePersonContract
    {
        public string Department { get; set; }
    }

    [Fact]
    public void SerializeInheritedDataContractClass_DerivedWithoutDataContract_UsesNonDataContractMode()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new DerivedPersonWithoutContract { Name = "John", Department = "IT" };
        var json = JsonSerializer.Serialize(obj, options);

        // Without DataContract on derived, all public members are included using their declared names
        Assert.Contains("Department", json);
        Assert.Contains("IT", json);
        // Name is inherited from BasePersonContract (public property)
        Assert.Contains("Name", json);
        Assert.Contains("John", json);
    }

    // ---- DataMember(IsRequired = true) tests ----

    [DataContract]
    public class ClassWithRequiredDataMember
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public int Age { get; set; }
    }

    [Fact]
    public void DeserializeClassWithDataMemberIsRequired_MissingProperty_ThrowsJsonException()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ClassWithRequiredDataMember>("{}", options));
    }

    [Fact]
    public void SerializeClassWithDataMemberIsRequired_PresentProperty_Succeeds()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new ClassWithRequiredDataMember { Name = "Test", Age = 30 };
        var json = JsonSerializer.Serialize(obj, options);

        Assert.Contains("Name", json);
        Assert.Contains("Test", json);

        var deserialized = JsonSerializer.Deserialize<ClassWithRequiredDataMember>(json, options);
        Assert.Equal("Test", deserialized.Name);
        Assert.Equal(30, deserialized.Age);
    }

    // ---- Write-only property tests ----

    [DataContract]
    public class ClassWithWriteOnlyProperty
    {
        private string _secret;

        [DataMember]
        public string WriteOnly
        {
            set { _secret = value; }
        }

        [DataMember]
        public string ReadWrite { get; set; }

        public string GetSecret() => _secret;
    }

    [Fact]
    public void SerializeClassWithWriteOnlyProperty_WriteOnlyExcludedFromOutput()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new ClassWithWriteOnlyProperty { ReadWrite = "visible" };
        var json = JsonSerializer.Serialize(obj, options);

        // Write-only property has no getter, so it must not appear in the serialized output
        Assert.DoesNotContain("WriteOnly", json);
        Assert.Contains("ReadWrite", json);
        Assert.Contains("visible", json);
    }

    [Fact]
    public void DeserializeClassWithWriteOnlyProperty_SetsPropertyValue()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = JsonSerializer.Deserialize<ClassWithWriteOnlyProperty>(@"{""WriteOnly"":""secret"",""ReadWrite"":""visible""}", options);

        Assert.Equal("secret", obj.GetSecret());
        Assert.Equal("visible", obj.ReadWrite);
    }

    // ---- Static member exclusion test ----

    [DataContract]
    public class ClassWithStaticMember
    {
        [DataMember]
        public string Name { get; set; }

        // Static members are not returned by GetProperties/GetFields with BindingFlags.Instance,
        // so they should never appear in the serialized output.
        [DataMember]
        public static string StaticName { get; set; } = "static_value";
    }

    [Fact]
    public void SerializeClassWithStaticMember_StaticMembersAreExcluded()
    {
        ClassWithStaticMember.StaticName = "should_not_appear";

        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new ClassWithStaticMember { Name = "instance_value" };
        var json = JsonSerializer.Serialize(obj, options);

        Assert.Contains("instance_value", json);
        Assert.DoesNotContain("should_not_appear", json);
        Assert.DoesNotContain("StaticName", json);
    }

    // ---- DataMember.Order with non-sequential values test ----

    [DataContract]
    public class ClassWithNonSequentialOrder
    {
        // Declared in reverse order; Order values are non-sequential to test sort correctness
        [DataMember(Order = 10)]
        public string Third { get; set; }

        [DataMember(Order = 0)]
        public string First { get; set; }

        [DataMember(Order = 5)]
        public string Second { get; set; }
    }

    [Fact]
    public void SerializeClassWithNonSequentialOrder_OrderIsPreserved()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var obj = new ClassWithNonSequentialOrder { First = "first", Second = "second", Third = "third" };
        var json = JsonSerializer.Serialize(obj, options);

        int firstPos = json.IndexOf("First", StringComparison.Ordinal);
        int secondPos = json.IndexOf("Second", StringComparison.Ordinal);
        int thirdPos = json.IndexOf("Third", StringComparison.Ordinal);

        Assert.True(firstPos < secondPos, "First (Order = 0) should appear before Second (Order = 5)");
        Assert.True(secondPos < thirdPos, "Second (Order = 5) should appear before Third (Order = 10)");
    }

    // ---- GetTypeInfo with non-Object kind test ----

    [Fact]
    public void GetTypeInfo_NonObjectKind_ReturnsTypeInfoUnchanged()
    {
        var resolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default;
        var options = new JsonSerializerOptions { TypeInfoResolver = resolver };

        // List<int> has Kind = Enumerable (not Object), so static GetTypeInfo should return it unchanged
        var listTypeInfo = resolver.GetTypeInfo(typeof(List<int>), options);
        Assert.NotEqual(System.Text.Json.Serialization.Metadata.JsonTypeInfoKind.Object, listTypeInfo.Kind);

        var result = System.Text.Json.Serialization.Metadata.DataContractResolver.GetTypeInfo(listTypeInfo);
        Assert.Same(listTypeInfo, result);
    }

    // ---- DataMember(IsRequired) on non-DataContract type has no effect test ----

    public class ClassWithDataMemberIsRequiredWithoutDataContract
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }

    [Fact]
    public void DeserializeClassWithDataMemberIsRequired_WithoutDataContract_NoRequiredEffect()
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        // Without [DataContract], DataMember(IsRequired=true) is not processed; missing property does NOT throw
        var obj = JsonSerializer.Deserialize<ClassWithDataMemberIsRequiredWithoutDataContract>("{}", options);
        Assert.Null(obj.Name);
    }
}