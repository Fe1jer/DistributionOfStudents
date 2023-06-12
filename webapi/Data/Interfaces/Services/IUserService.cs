using webapi.Data.Models;
using webapi.ViewModels;

namespace webapi.Data.Interfaces.Services
{
    public interface IUserService
    {
        Task<object?> Authenticate(LoginVM model);
    }
}
