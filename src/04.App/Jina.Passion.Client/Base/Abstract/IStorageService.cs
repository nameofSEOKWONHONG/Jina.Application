namespace Jina.Passion.Client.Base.Abstract;

/// <summary>
/// 공통 브라우저 저장 서비스, 로컬 스토리지를 사용
/// </summary>
public interface IStorageService
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);

    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value);

    Task<string> GetOnceAsync(string key);

    Task RemoveAsync(string key);
    Task RemoveAllAsync(string[] keys);

    Task ClearAsync();
}