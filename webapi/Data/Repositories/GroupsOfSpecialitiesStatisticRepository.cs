using ChartJSCore.Models;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Repositories
{
    public class GroupsOfSpecialitiesStatisticRepository : Repository<GroupOfSpecialitiesStatistic>, IGroupsOfSpecialitiesStatisticRepository
    {
        public GroupsOfSpecialitiesStatisticRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            GroupOfSpecialitiesStatistic? statistic = await GetByIdAsync(id);
            if (statistic != null)
            {
                await DeleteAsync(statistic);
            }
        }

        public async Task<List<GroupOfSpecialitiesStatistic>> GetAllAsync(int groupId)
        {
            return (await GetAllAsync()).Where(i => i.GroupOfSpecialties.Id == groupId).OrderBy(i=>i.Date).ToList();
        }

        public async Task<GroupOfSpecialitiesStatistic?> GetByGroupAndDateAsync(int groupId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.GroupOfSpecialties.Id == groupId && i.Date == date);
        }
    }
}
