using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IBaseApiClient<T> where T : class
    {

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(object id);

        Task<KeyValuePair<bool, string>> SaveAsync(T obj);

        Task<KeyValuePair<bool, string>> UpdateAsync(object id, T obj);

        Task<KeyValuePair<bool, string>> DeleteAsync(object id);

    }
}
