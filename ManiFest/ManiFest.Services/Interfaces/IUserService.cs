using ManiFest.Services.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManiFest.Model.Responses;
using ManiFest.Model.Requests;
using ManiFest.Model.SearchObjects;
using ManiFest.Services.Services;

namespace ManiFest.Services.Interfaces
{
    public interface IUserService : IService<UserResponse, UserSearchObject>
    {
        Task<UserResponse?> AuthenticateAsync(UserLoginRequest request);
        Task<UserResponse> CreateAsync(UserUpsertRequest request);
        Task<UserResponse?> UpdateAsync(int id, UserUpsertRequest request);
        Task<bool> DeleteAsync(int id);
    }
}