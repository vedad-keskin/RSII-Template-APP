using CallTaxi.Services.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using CallTaxi.Model.Responses;
using CallTaxi.Model.Requests;
using CallTaxi.Model.SearchObjects;

namespace CallTaxi.Services.Interfaces
{
    public interface IService<T, TSearch> where T : class where TSearch : BaseSearchObject
    {
        Task<PagedResult<T>> GetAsync(TSearch search);
        Task<T?> GetByIdAsync(int id);
    }
}