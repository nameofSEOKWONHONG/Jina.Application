using eXtensionSharp;
using Jina.Domain.Example;
using Jina.Domain.SharedKernel;
using Microsoft.AspNetCore.Components;

namespace Jina.Passion.Client.Pages.Weather.ViewModelService
{
    public class WeatherViewModel
    {
        public WeatherService WeatherService { get; set; }

        public List<WeatherForecast> WeatherForecasts { get; set; }
        public IEnumerable<WeatherForecast> SelectedItems { get; set; }
        public WeatherForecast SelectedItem { get; set; }

        public WeatherViewModel(WeatherService service)
        {
            this.WeatherService = service;
        }

        public async Task InitializeAsync()
        {
            //1, 10
            this.WeatherForecasts = await this.WeatherService.GetWeathersAsync(new PaginatedRequest<WeatherForecast>());
        }

        public async Task GetWeathersAsync(PaginatedRequest<WeatherForecast> request)
        {
            //1, 10
            this.WeatherForecasts = await this.WeatherService.GetWeathersAsync(new PaginatedRequest<WeatherForecast>());
        }

        public async Task GetWeatherAsync(int id)
        {
            this.SelectedItem = await this.WeatherService.GetWeatherAsync(id);
        }

        public async Task SaveAsync(WeatherForecast item)
        {
            if (item.Id <= 0)
            {
                //add
            }
            else
            {
                //update
            }

            var result = await WeatherService.SaveAsync(item);
            if (result.Succeeded)
            {
                this.WeatherForecasts.Insert(0, item);
            }
        }

        public async Task RemoveAsync()
        {
            var exist = this.WeatherForecasts.First(x => x.Id == this.SelectedItem.Id);
            if (exist.xIsEmpty()) return;

            var result = await this.WeatherService.RemoveAsync(exist);
            if (result.Succeeded)
            {
                this.WeatherForecasts.Remove(exist);
            }
        }

        public async Task RemoveRangeAsync()
        {
            await this.WeatherService.RemoveRangeAsync(this.SelectedItems);

            foreach (var item in this.SelectedItems)
            {
                this.WeatherForecasts.Remove(item);
            }
            await Task.Delay(1000);
        }
    }
}