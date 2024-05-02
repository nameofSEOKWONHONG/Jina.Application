using System.Threading.Tasks;

namespace Jina.Passion.Client.Base.Abstract
{
    /// <summary>
    /// 세션 브라우저 저장 서비스
    /// </summary>
    public interface ISessionStorageService
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
    

}