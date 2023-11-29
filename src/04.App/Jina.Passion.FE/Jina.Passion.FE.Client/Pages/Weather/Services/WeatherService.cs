﻿using Jina.Domain.Example;
using Jina.Domain.SharedKernel;
using Jina.Passion.FE.Client.Base;
using System.Net.Http.Json;

namespace Jina.Passion.FE.Client.Pages.Weather.Services
{
    public class WeatherService : FeServiceBase
    {
        public List<WeatherForecast> WeatherForecasts { get; set; }
        public WeatherForecast SelectedItem { get; set; }

        public WeatherService(HttpClient httpClient) : base(httpClient)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            WeatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToList();
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeathersAsync(PagenatedRequest<WeatherForecast> request)
        {
            // Simulate asynchronous loading to demonstrate a loading indicator
            await Task.Delay(500);

            //call api
            var weathers = await this.Client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("weathers.json");
            weathers.Where(m => m.Id == request.SearchOption.Id)
                .Take(request.PageSize)
                .Skip(request.PageSize * request.PageNo).ToList();

            return WeatherForecasts;
        }

        public async Task<WeatherForecast> GetWeatherAsync(int id)
        {
            var res = await this.Client.GetFromJsonAsync<WeatherForecast>("get");
            return res;
        }

        public Task SaveAsync(WeatherForecast item)
        {
            if (item.Id <= 0)
            {
                //add
                //call api
                this.WeatherForecasts.Insert(0, item);
            }
            else
            {
                //update
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(WeatherForecast item)
        {
            var exist = this.WeatherForecasts.First(m => m.Id == item.Id);
            //call api

            this.WeatherForecasts.Remove(exist);
            return Task.CompletedTask;
        }
    }
}