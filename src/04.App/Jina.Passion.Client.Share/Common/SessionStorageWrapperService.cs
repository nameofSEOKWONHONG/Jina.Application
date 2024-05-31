using eXtensionSharp;

namespace Jina.Passion.Client.Share.Common;

public class SessionStorageWrapperService : ISessionStorageWrapperService
{
    private readonly Blazored.SessionStorage.ISessionStorageService _service;
    public SessionStorageWrapperService(Blazored.SessionStorage.ISessionStorageService service)
    {
        _service = service;
    }
    
    public async Task ClearAsync()
    {
        await _service.ClearAsync();
    }
    
    public async Task<string> GetOnceAsync(string key)
    {
        var value =  await _service.GetItemAsStringAsync(key);
        if(value.xIsNotEmpty())
        {
            await _service.RemoveItemAsync(key);
        }
        return value;
    }
    
    public async Task<T> GetAsync<T>(string key)
    {
        var value =  await _service.GetItemAsync<T>(key);
        return value;
    }
    
    public async Task SetAsync<T>(string key, T value)
    {
        await _service.SetItemAsync<T>(key, value);
    }
    
    public async Task<string> GetAsync(string key)
    {
        var value = await _service.GetItemAsStringAsync(key);            
        return value;
    }
    
    public async Task SetAsync(string key, string value)
    {
        await _service.SetItemAsStringAsync(key, value);
    }
    
    public async Task RemoveAsync(string key)
    {
        await _service.RemoveItemAsync(key);
    }
            
    public async Task RemoveAllAsync(string[] keys)
    {
        var tasks = keys.Select(RemoveAsync);
        await Task.WhenAll(tasks);
    }  
}