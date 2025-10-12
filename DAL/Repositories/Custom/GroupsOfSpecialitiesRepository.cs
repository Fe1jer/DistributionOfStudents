using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using DAL.Specifications;

namespace DAL.Repositories.Custom
{
    public class GroupsOfSpecialitiesRepository(ApplicationDbContext appDBContext) : Repository<GroupOfSpecialities>(appDBContext), IGroupsOfSpecialitiesRepository
    {
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
