using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IGroupsOfSpecialitiesRepository : IRepository<GroupOfSpecialities>
    {
        Task<List<GroupOfSpecialities>> GetByFacultyAsync(string facultyUrl, int year);
    }
}
