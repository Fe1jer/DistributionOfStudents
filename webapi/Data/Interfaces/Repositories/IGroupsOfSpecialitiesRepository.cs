using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IGroupsOfSpecialitiesRepository
    {
        Task<GroupOfSpecialties?> GetByIdAsync(int groupOfSpecialtiesId);
        Task<GroupOfSpecialties?> GetByIdAsync(int id, ISpecification<GroupOfSpecialties> specification);
        Task<List<GroupOfSpecialties>> GetAllAsync();
        Task<List<GroupOfSpecialties>> GetAllAsync(ISpecification<GroupOfSpecialties> specification);
        Task AddAsync(GroupOfSpecialties groupOfSpecialties);
        Task UpdateAsync(GroupOfSpecialties groupOfSpecialties);
        Task DeleteAsync(int id);
    }
}
