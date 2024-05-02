namespace Jina.Domain.Example;

public class WeatherForecastRequest
{
    public string City { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}