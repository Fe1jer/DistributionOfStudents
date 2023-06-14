using webapi.Data.Models;
using webapi.ViewModels;
using webapi.ViewModels.Users;

namespace webapi.Data.Interfaces.Services
{
    public interface IUserService
    {
        Task<object?> Authenticate(LoginVM model);
        Task<User?> Registration(CreateUserVM model);
    }
}
