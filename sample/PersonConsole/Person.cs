[DataContract]
public class Person
{
    [DataMember(EmitDefaultValue = false, Order = 2)]
    public string? FullName { get; set; }

    [DataMember(Order = 1)]
    public int Age { get; set; }

    [IgnoreDataMember]
    public string? DoNotShow { get; set; }
}