using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Jina.Domain.Example;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Jina.Passion.Client.Pages.Weather.Services
{
    public class WeatherService : ServiceBase
    {
        public WeatherService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<List<WeatherForecastDto>> GetWeathersAsync(PaginatedRequest<WeatherForecastDto> request)
        {
            // Simulate asynchronous loading to demonstrate a loading indicator
            await Task.Delay(500);

            //call api
            //var weathers = await this.Client.GetFromJsonAsync<IEnumerable<WeatherForecastDto>>("weathers.json");
            //weathers.Where(m => m.Id == request.SearchOption.Id)
            //    .Take(request.PageSize)
            //    .Skip(request.PageSize * request.PageNo).ToList();

            var startDate = DateTime.Now;
            var cities = new[] { "서울", "춘천", "원주", "인천", "경기" };
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Id = index,
                City = cities[index - 1],
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToList();

            return result;
        }

        public async Task<WeatherForecastDto> GetWeatherAsync(int id)
        {
            var result = await Client.GetFromJsonAsync<WeatherForecastDto>("get");
            return result;
        }

        public async Task<IResultBase> SaveAsync(WeatherForecastDto item)
        {
            //call api

            var result = await Result.SuccessAsync();
            return result;
        }

        public async Task<IResultBase> RemoveAsync(WeatherForecastDto item)
        {
            //call api

            var result = await Result.SuccessAsync();
            return result;
        }

        public async Task<IResultBase> RemoveRangeAsync(IEnumerable<WeatherForecastDto> items)
        {
            //call api

            var result = await Result.SuccessAsync();
            return result;
        }
    }
}