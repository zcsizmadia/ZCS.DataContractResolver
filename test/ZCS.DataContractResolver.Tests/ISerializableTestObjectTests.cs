using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Tests.TestObjects;
using Newtonsoft.Json.Tests.TestObjects.Organization;
using Xunit;

public class ISerializableTestObjectTests
{
    // System.Text.Json ignores ISerializable, so GetObjectData is only reachable
    // through Newtonsoft. This pins its output so refactoring the fixture cannot
    // silently change what the ported tests serialize.
    [Fact]
    public void GetObjectDataWritesEveryMember()
    {
        var testObject = new ISerializableTestObject(
            "stringValue",
            123,
            new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero),
            new Person());

        string json = JsonConvert.SerializeObject(testObject);

        Assert.Equal(
            "{\"stringValue\":\"stringValue\"," +
            "\"intValue\":123," +
            "\"dateTimeOffsetValue\":\"2020-01-02T03:04:05+00:00\"," +
            "\"personValue\":{\"Name\":null,\"BirthDate\":\"0001-01-01T00:00:00\",\"LastModified\":\"0001-01-01T00:00:00\"}," +
            "\"nullPersonValue\":null," +
            "\"nullableInt\":null," +
            "\"booleanValue\":false," +
            "\"byteValue\":0," +
            "\"charValue\":\"\\u0000\"," +
            "\"dateTimeValue\":\"0001-01-01T00:00:00Z\"," +
            "\"decimalValue\":0.0," +
            "\"shortValue\":0," +
            "\"longValue\":0," +
            "\"sbyteValue\":0," +
            "\"floatValue\":0.0," +
            "\"ushortValue\":0," +
            "\"uintValue\":0," +
            "\"ulongValue\":0}",
            json);
    }
}
