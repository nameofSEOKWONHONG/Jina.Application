using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jina.Domain.Entity.Example
{
	[Table("WeatherForecast", Schema = "example")]
    public class WeatherForecast : TenantBase
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        [Comment("도시명")]
        public string City { get; set; }

        [Comment("날짜")]
        public DateTime? Date { get; set; }

        [Comment("섭씨온도")]
        public int? TemperatureC { get; set; }

        [MaxLength(300)]
        [Comment("요약")]
        public string Summary { get; set; }
    }

    public class WeatherForecastBuilder : IModelBuilder
    {
        public WeatherForecast Entity { get; set; }

        public void Build(ModelBuilder builder)
        {
            builder.Entity<WeatherForecast>(entity =>
                {
                    entity.HasKey(m => new { m.TenantId, m.Id }); ;
                    entity.Property(m => m.Id)
                        .ValueGeneratedOnAdd();
                    entity.Property(m => m.City)
                        .HasMaxLength(100);
                    entity.Property(m => m.Summary)
                        .HasMaxLength(300);                    
                });
        }
    }
}