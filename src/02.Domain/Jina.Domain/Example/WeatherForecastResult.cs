using FluentValidation;
using Jina.Lang.Abstract;
using Jina.Validate;
using Jina.Validate.RuleValidate.Impl;

namespace Jina.Domain.Example
{
    public class WeatherForecastResult : DtoBase
    {
        public long Id { get; set; }
        public string City { get; set; }
        public DateTime? Date { get; set; }
        public string Summary { get; set; }
        public int? TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
    
    public class WeatherForecastResultValidator : Validator<WeatherForecastResult>
    {
        public WeatherForecastResultValidator(ILocalizer localizer) : base(localizer)
        {
            NotEmpty(m => m.City);
            NotEmpty(m => m.Date);
            NotEmpty(m => m.Summary);
            NotEmpty(m => m.TemperatureC);
        }
    }    
}