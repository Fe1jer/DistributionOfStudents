using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IGroupsOfSpecialitiesStatisticRepository : IRepository<GroupOfSpecialitiesStatistic>
    {
        Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(Guid groupId, DateTime date);
        Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync(Guid groupId);
    }
}
