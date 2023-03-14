using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
{
    public interface ISpecialitiesRepository
    {
        Task<Speciality?> GetByIdAsync(int specialityId);
        Task<List<Speciality>> GetAllAsync();
        Task<List<Speciality>> GetAllAsync(ISpecification<Speciality> specification);
        Task AddAsync(Speciality speciality);
        Task UpdateAsync(Speciality speciality);
        Task DeleteAsync(int id);
    }
}
