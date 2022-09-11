using System.Runtime.Serialization;

namespace WeatherForecastWebApi;

[DataContract]
public class WeatherForecast
{
    [DataMember(EmitDefaultValue = false)]
    public DateOnly Date { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int TemperatureC { get; set; }

    [IgnoreDataMember]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [DataMember(EmitDefaultValue = false)]
    public string? Summary { get; set; }
}
