using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
{
    public class GroupsOfSpecialitiesRepository : Repository<GroupOfSpecialties>, IGroupsOfSpecialitiesRepository
    {
        public GroupsOfSpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            GroupOfSpecialties? groupOfSpecialties = await GetByIdAsync(id);
            if (groupOfSpecialties != null)
            {
                await DeleteAsync(groupOfSpecialties);
            }
        }
    }
}
