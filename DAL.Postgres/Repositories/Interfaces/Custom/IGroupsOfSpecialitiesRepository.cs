using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IGroupsOfSpecialitiesRepository
    {
        Task<GroupOfSpecialities?> GetByIdAsync(Guid groupOfSpecialtiesId);
        Task<GroupOfSpecialities?> GetByIdAsync(Guid id, ISpecification<GroupOfSpecialities> specification);
        Task<List<GroupOfSpecialities>> GetAllAsync();
        Task<List<GroupOfSpecialities>> GetAllAsync(ISpecification<GroupOfSpecialities> specification);
        Task AddAsync(GroupOfSpecialities groupOfSpecialties);
        Task UpdateAsync(GroupOfSpecialities groupOfSpecialties);
        Task DeleteAsync(Guid id);
    }
}
