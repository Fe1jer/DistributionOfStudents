using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
{
    public interface IFacultiesRepository
    {
        Task<Faculty> GetByIdAsync(int facultyId);
        Task<Faculty> GetByIdAsync(int id, ISpecification<Faculty> specification);
        Task<List<Faculty>> GetAllAsync();
        Task<List<Faculty>> GetAllAsync(ISpecification<Faculty> specification);
        Task AddAsync(Faculty faculty);
        Task UpdateAsync(Faculty faculty);
        Task DeleteAsync(int id);
    }
}
