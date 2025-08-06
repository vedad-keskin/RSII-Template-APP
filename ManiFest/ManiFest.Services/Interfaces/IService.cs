using ManiFest.Services.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManiFest.Model.Responses;
using ManiFest.Model.Requests;
using ManiFest.Model.SearchObjects;

namespace ManiFest.Services.Interfaces
{
    public interface IService<T, TSearch> where T : class where TSearch : BaseSearchObject
    {
        Task<PagedResult<T>> GetAsync(TSearch search);
        Task<T?> GetByIdAsync(int id);
    }
}