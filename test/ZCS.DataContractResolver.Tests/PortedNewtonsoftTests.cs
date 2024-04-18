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
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };

        var json = JsonSerializer.Serialize(new RequiredObject { AllowNullProperty = "Test" }, options);

        Assert.Contains("AllowNullProperty", json);
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
}