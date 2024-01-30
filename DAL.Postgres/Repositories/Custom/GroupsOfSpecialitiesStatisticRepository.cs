using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class GroupsOfSpecialitiesStatisticRepository : Repository<GroupOfSpecialitiesStatistic>, IGroupsOfSpecialitiesStatisticRepository
    {
        public GroupsOfSpecialitiesStatisticRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            GroupOfSpecialitiesStatistic? statistic = await GetByIdAsync(id);
            if (statistic != null)
            {
                await DeleteAsync(statistic);
            }
        }

        public async Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync(Guid groupId)
        {
            return (await GetAllAsync()).Where(i => i.GroupOfSpecialties.Id == groupId).OrderBy(i => i.Date).ToList();
        }

        public async Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(Guid groupId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.GroupOfSpecialties.Id == groupId && i.Date == date);
        }
    }
}
