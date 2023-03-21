using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
{
    public interface IAdmissionsRepository
    {
        Task<Admission?> GetByIdAsync(int admissionId);
        Task<Admission?> GetByIdAsync(int id, ISpecification<Admission> specification);
        Task<List<Admission>> GetAllAsync();
        Task<List<Admission>> GetAllAsync(ISpecification<Admission> specification);
        Task AddAsync(Admission admission);
        Task UpdateAsync(Admission admission);
        Task DeleteAsync(int id);
        Task<List<Admission>> SearchByStudentsAsync(string? searchText, ISpecification<Admission> specification);
    }
}
