using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories.Custom
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
            return (await GetAllAsync()).Where(i => i.GroupOfSpecialtiesId == groupId).OrderBy(i => i.Date).ToList();
        }

        public async Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(Guid groupId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.GroupOfSpecialtiesId == groupId && i.Date.ToLocalTime().Date == date.Date);
        }
    }
}
