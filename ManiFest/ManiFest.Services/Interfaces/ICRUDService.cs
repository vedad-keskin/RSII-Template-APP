using ManiFest.Services.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManiFest.Model.Responses;
using ManiFest.Model.Requests;
using ManiFest.Model.SearchObjects;

namespace ManiFest.Services.Interfaces
{
    public interface ICRUDService<T, TSearch, TInsert, TUpdate> : IService<T, TSearch> where T : class where TSearch : BaseSearchObject where TInsert : class where TUpdate : class
    {
        Task<T> CreateAsync(TInsert request);
        Task<T?> UpdateAsync(int id, TUpdate request);
        Task<bool> DeleteAsync(int id);
    }
}