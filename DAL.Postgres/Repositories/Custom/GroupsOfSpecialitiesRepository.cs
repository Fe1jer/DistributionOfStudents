using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class GroupsOfSpecialitiesRepository : Repository<GroupOfSpecialities>, IGroupsOfSpecialitiesRepository
    {
        public GroupsOfSpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            GroupOfSpecialities? groupOfSpecialties = await GetByIdAsync(id);
            if (groupOfSpecialties != null)
            {
                await DeleteAsync(groupOfSpecialties);
            }
        }
    }
}
