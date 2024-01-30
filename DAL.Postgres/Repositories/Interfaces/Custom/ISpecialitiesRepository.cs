using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface ISpecialitiesRepository
    {
        Task<Speciality?> GetByIdAsync(Guid specialityId);
        Task<List<Speciality>> GetAllAsync();
        Task<List<Speciality>> GetAllAsync(ISpecification<Speciality> specification);
        Task AddAsync(Speciality speciality);
        Task UpdateAsync(Speciality speciality);
        Task DeleteAsync(Guid id);
    }
}
