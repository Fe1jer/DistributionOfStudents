using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface ISubjectsRepository
    {
        Task<Subject?> GetByIdAsync(Guid subjectId);
        Task<List<Subject>> GetAllAsync();
        Task<List<Subject>> GetAllAsync(ISpecification<Subject> specification);
        Task AddAsync(Subject subject);
        Task UpdateAsync(Subject subject);
        Task DeleteAsync(Guid id);
    }
}
