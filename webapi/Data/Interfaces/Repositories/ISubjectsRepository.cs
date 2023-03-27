using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface ISubjectsRepository
    {
        Task<Subject?> GetByIdAsync(int subjectId);
        Task<List<Subject>> GetAllAsync();
        Task<List<Subject>> GetAllAsync(ISpecification<Subject> specification);
        Task AddAsync(Subject subject);
        Task UpdateAsync(Subject subject);
        Task DeleteAsync(int id);
    }
}
