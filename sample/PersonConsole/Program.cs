using PersonConsole;

var options = new JsonSerializerOptions()
{
    TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
};

Person person = new()
{
    FullName = "John Doe",
    Age = 21,
    DoNotShow = "SECRET!"
};

// Serialize with System.Text.Json
string json = JsonSerializer.Serialize(person, options);

Console.WriteLine(json);
