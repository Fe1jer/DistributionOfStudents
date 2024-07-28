using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IGroupsOfSpecialitiesStatisticRepository : IRepository<GroupOfSpecialitiesStatistic>
    {
        Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(Guid groupId, DateTime date);
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync(Guid groupId);
    }
}
