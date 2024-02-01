using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Models;

namespace Jina.Passion.Client.Services;

public interface IChartService
{
    Task<ChartDataItem[]> GetVisitDataAsync();
    Task<ChartDataItem[]> GetVisitData2Async();
    Task<ChartDataItem[]> GetSalesDataAsync();
    Task<RadarDataItem[]> GetRadarDataAsync();
}

public class ChartService : ServiceBase, IChartService
{
    public ChartService(HttpClient client) : base(client)
    {
    }

    public async Task<ChartDataItem[]> GetVisitDataAsync()
    {
        return (await GetChartDataAsync()).VisitData;
    }

    public async Task<ChartDataItem[]> GetVisitData2Async()
    {
        return (await GetChartDataAsync()).VisitData2;
    }

    public async Task<ChartDataItem[]> GetSalesDataAsync()
    {
        return (await GetChartDataAsync()).SalesData;
    }

    public async Task<RadarDataItem[]> GetRadarDataAsync()
    {
        return (await GetChartDataAsync()).RadarData;
    }

    private async Task<ChartData> GetChartDataAsync()
    {
        return await this.Client.GetFromJsonAsync<ChartData>("https://localhost:7007/data/fake_chart_data.json");
    }
}