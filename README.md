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
var options = new JsonSerializerOptions()
{
    TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
};

Person person = new Person();

string json = JsonSerializer.Serialize(person, options);
```

License
---
This library is under the MIT License.
