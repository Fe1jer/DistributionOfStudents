using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
{
    public interface IGroupsOfSpecialitiesStatisticRepository
    {
        Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(int groupId, DateTime date);
        Task<GroupOfSpecialitiesStatistic?> GetByIdAsync(int statisticId);
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync();
        Task AddAsync(GroupOfSpecialitiesStatistic statistic);
        Task UpdateAsync(GroupOfSpecialitiesStatistic statistic);
        Task DeleteAsync(int id);
    }
}
