using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IFacultiesRepository
    {
        Task<Faculty?> GetByIdAsync(int facultyId);
        Task<Faculty?> GetByIdAsync(int id, ISpecification<Faculty> specification);
        Task<Faculty?> GetByShortNameAsync(string name);
        Task<Faculty?> GetByShortNameAsync(string name, ISpecification<Faculty> specification);
        Task<List<Faculty>> GetAllAsync();
        Task<List<Faculty>> GetAllAsync(ISpecification<Faculty> specification);
        Task AddAsync(Faculty faculty);
        Task UpdateAsync(Faculty faculty);
        Task DeleteAsync(int id);
    }
}
