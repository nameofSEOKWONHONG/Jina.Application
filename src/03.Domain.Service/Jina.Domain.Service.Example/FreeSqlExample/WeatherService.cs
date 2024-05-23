using Jina.Base.Service.Abstract;
using Jina.Domain.Entity.Account;
using Jina.Domain.Entity.Example;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.FreeSqlExample;

public interface IWeatherService : IScopeService
{
    Task<WeatherForecast> Get(string tenantId, int id);
    Task<int> Update(WeatherUpdateRequest request);
}

public class WeatherUpdateRequest
{
    public string TenantId { get; set; } 
    public int Id { get; set; } 
    public string City { get; set; }
}

public class WeatherService : IWeatherService
{
    private readonly IFreeSql _sql;
    public WeatherService(IFreeSql sql)
    {
        _sql = sql;
    }

    public async Task<WeatherForecast> Get(string tenantId, int id)
    {
        var result1 = await _sql.Select<WeatherForecast, User>()
            .InnerJoin((forecast, user) => forecast.CreatedBy == user.Id)
            .Where(m => m.t1.TenantId == tenantId)
            .Where(m => m.t1.Id == id)
            .WithLock(SqlServerLock.NoLock | SqlServerLock.NoWait)
            .FirstAsync(s => new
            {
                s.t1.Id,
                s.t1.City,
                s.t2.FirstName
            });
        
        var result = await _sql.Select<WeatherForecast>()
            .Where(m => m.TenantId == tenantId)
            .Where(m => m.Id == id)
            .WithLock(SqlServerLock.NoLock | SqlServerLock.NoWait)
            .FirstAsync();

        return result;
    }

    public async Task<int> Update(WeatherUpdateRequest request)
    {
        var result = await _sql.Update<WeatherForecast>()
            .AsTable(s => $"{s}s")
            .Where(m => m.TenantId == request.TenantId)
            .Where(m => m.Id == request.Id)
            .Set(m => m.City, request.City)
            .ExecuteAffrowsAsync();

        return result;
    }
}