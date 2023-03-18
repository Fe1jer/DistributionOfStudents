using ChartJSCore.Models;
using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Repositories
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
