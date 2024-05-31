using Blazored.LocalStorage;
using eXtensionSharp;
using Jina.Passion.Client.Share.Common;

namespace Jina.Passion.Client.Base;

/// <summary>
/// 테스트 및 공통 저장
/// </summary>
public class LocalStorageWrapperService : ISessionStorageWrapperService, ILocalStorageWrapperService
{
    private readonly ILocalStorageService _service;
    public LocalStorageWrapperService(ILocalStorageService localStorageService)
    {
        _service = localStorageService;
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

    public async Task<string> GetOnceAsync(string key)
    {
        var value =  await _service.GetItemAsStringAsync(key);
        if(value.xIsNotEmpty())
        {
            await _service.RemoveItemAsync(key);
        }
        return value;
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

    public async Task ClearAsync()
    {
        await _service.ClearAsync();
    }
}