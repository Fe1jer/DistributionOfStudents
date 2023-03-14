using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
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
