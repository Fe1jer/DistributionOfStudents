using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
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
