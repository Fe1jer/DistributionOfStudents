using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Specifications;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories.Custom
{
    public class GroupsOfSpecialitiesRepository : Repository<GroupOfSpecialities>, IGroupsOfSpecialitiesRepository
    {
        public GroupsOfSpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public Task<List<GroupOfSpecialities>> GetByFacultyAsync(string facultyUrl, int year)
        {
            return GetAllAsync(new GroupsOfSpecialitiesSpecification().WhereFaculty(facultyUrl).WhereYear(year));
        }

        public async Task DeleteAsync(Guid id)
        {
            GroupOfSpecialities? GroupOfSpecialities = await GetByIdAsync(id);
            if (GroupOfSpecialities != null)
            {
                await DeleteAsync(GroupOfSpecialities);
            }
        }
    }
}
