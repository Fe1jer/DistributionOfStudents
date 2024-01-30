using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IFacultiesRepository
    {
        Task<Faculty?> GetByIdAsync(Guid facultyId);
        Task<Faculty?> GetByIdAsync(Guid id, ISpecification<Faculty> specification);
        Task<Faculty?> GetByShortNameAsync(string name);
        Task<Faculty?> GetByShortNameAsync(string name, ISpecification<Faculty> specification);
        Task<List<Faculty>> GetAllAsync();
        Task<List<Faculty>> GetAllAsync(ISpecification<Faculty> specification);
        Task AddAsync(Faculty faculty);
        Task UpdateAsync(Faculty faculty);
        Task DeleteAsync(Guid id);
    }
}
