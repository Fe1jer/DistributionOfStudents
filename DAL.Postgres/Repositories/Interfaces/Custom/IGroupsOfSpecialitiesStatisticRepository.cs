using DAL.Postgres.Entities;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IGroupsOfSpecialitiesStatisticRepository
    {
        Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(Guid groupId, DateTime date);
        Task<GroupOfSpecialitiesStatistic?> GetByIdAsync(Guid statisticId);
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync();
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync(Guid groupId);
        Task AddAsync(GroupOfSpecialitiesStatistic statistic);
        Task UpdateAsync(GroupOfSpecialitiesStatistic statistic);
        Task DeleteAsync(Guid id);
    }
}
