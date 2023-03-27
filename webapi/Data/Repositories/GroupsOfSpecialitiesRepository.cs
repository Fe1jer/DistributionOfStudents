using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;

namespace webapi.Data.Repositories
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
