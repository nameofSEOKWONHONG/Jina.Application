using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Example
{
    public class DtoBase
    {
        public string CreatedName { get; set; }
        public string LastModifiedName { get; set; }
    }

    public class WeatherForecastDto : DtoBase
    {
        public long Id { get; set; }
        public string City { get; set; }
        public DateTime? Date { get; set; }
        public int? TemperatureC { get; set; }
        public string Summary { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public class WeatherForecastValidator : AbstractValidator<WeatherForecastDto>
        {
            public WeatherForecastValidator()
            {
                RuleFor(m => m.Date)
                    .NotEmpty();

                RuleFor(m => m.TemperatureC)
                    .NotEmpty();

                RuleFor(m => m.Summary)
                    .NotEmpty();
            }
        }
    }
}