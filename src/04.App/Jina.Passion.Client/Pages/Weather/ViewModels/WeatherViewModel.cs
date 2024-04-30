using eXtensionSharp;
using Jina.Domain.Example;
using Jina.Domain.Shared;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Pages.Weather.Services;

namespace Jina.Passion.Client.Pages.Weather.ViewModels
{
    public class WeatherViewModel : ViewModelBase<WeatherForecastRequest>
    {
        public WeatherService WeatherService { get; set; }

        public WeatherViewModel(WeatherService service)
        {
            this.WeatherService = service;
        }

        public async Task<List<WeatherForecastRequest>> GetWeathersAsync(PaginatedRequest<WeatherForecastRequest> request)
        {
            //1, 10
            this.Items = await this.WeatherService.GetWeathersAsync(new PaginatedRequest<WeatherForecastRequest>());
            return this.Items;
        }

        public async Task GetWeatherAsync(int id)
        {
            this.SelectedItem = await this.WeatherService.GetWeatherAsync(id);
        }

        public async Task SaveAsync(WeatherForecastRequest item)
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
                this.Items.Insert(0, item);
            }
        }

        public async Task RemoveAsync()
        {
            var exist = this.Items.First(x => x.Id == this.SelectedItem.Id);
            if (exist.xIsEmpty()) return;

            var result = await this.WeatherService.RemoveAsync(exist);
            if (result.Succeeded)
            {
                this.Items.Remove(exist);
            }
        }

        public async Task RemoveRangeAsync()
        {
            await this.WeatherService.RemoveRangeAsync(this.SelectedItems);

            foreach (var item in this.SelectedItems)
            {
                this.Items.Remove(item);
            }
            await Task.Delay(1000);
        }
    }
}