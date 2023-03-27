using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IGroupsOfSpecialitiesStatisticRepository
    {
        Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(int groupId, DateTime date);
        Task<GroupOfSpecialitiesStatistic?> GetByIdAsync(int statisticId);
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync();
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync(int groupId);
        Task AddAsync(GroupOfSpecialitiesStatistic statistic);
        Task UpdateAsync(GroupOfSpecialitiesStatistic statistic);
        Task DeleteAsync(int id);
    }
}
