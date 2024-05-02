using Jina.Domain.Example;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Common.Infra;

namespace Jina.Passion.Client.Pages.Weather.Services
{
    public class WeatherService : ServiceBase
    {
        public WeatherService(IRestClient httpClient) : base(httpClient)
        {
        }

        public async Task<List<WeatherForecastResult>> GetWeathersAsync(PaginatedRequest<WeatherForecastResult> request)
        {
            // Simulate asynchronous loading to demonstrate a loading indicator
            await Task.Delay(500);

            //call api
            //var weathers = await this.Client.GetFromJsonAsync<IEnumerable<WeatherForecastRequest>>("weathers.json");
            //weathers.Where(m => m.Id == request.SearchOption.Id)
            //    .Take(request.PageSize)
            //    .Skip(request.PageSize * request.PageNo).ToList();

            var startDate = DateTime.Now;
            var cities = new[] { "서울", "춘천", "원주", "인천", "경기" };
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecastResult
            {
                Id = index,
                City = cities[index - 1],
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToList();

            return result;
        }

        public async Task<WeatherForecastResult> GetWeatherAsync(int id)
        {
            var url = $"api/example/get/{id}";
            var result = await Client.ExecuteAsync<int, IResults<WeatherForecastResult>>(HttpMethod.Get, url, 0);
            return result.Data;
        }

        public async Task<IResults> SaveAsync(WeatherForecastResult item)
        {
            //call api

            var result = await Results.SuccessAsync();
            return result;
        }

        public async Task<IResults> RemoveAsync(WeatherForecastResult item)
        {
            //call api

            var result = await Results.SuccessAsync();
            return result;
        }

        public async Task<IResults> RemoveRangeAsync(IEnumerable<WeatherForecastResult> items)
        {
            //call api

            var result = await Results.SuccessAsync();
            return result;
        }
    }
}