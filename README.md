Intro
---
System.Text.Json serializer does not support DataContact and DataMember attributes prior .NET 7.0. System.Text.Json v7.x+ supports custom type resolvers. This library adds support for DataContract and DataMember attributes using a custom resolver.

Usage
---
```csharp
[DataContract]
public class Person
{
    [DataMember(EmitDefaultValue = false, Order = 2)]
    public string FullName { get; set; }

    [DataMember(Order = 1)]
    public int Age { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public Dictionary<string, string> Dict { get; set; }

    [IgnoreDataMember]
    public string DoNotShow { get; set; }
}

// Serialize with System.Text.Json
var options = new System.Text.Json.JsonSerializerOptions()
{
    TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
};

Person person = new Person();

string json = System.Text.JsonJsonSerializer.Serialize(person, options);
```

License
---
This library is under the MIT License.
