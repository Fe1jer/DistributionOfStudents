using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IGroupsOfSpecialitiesRepository : IRepository<GroupOfSpecialities>
    {
        Task<List<GroupOfSpecialities>> GetByFacultyAsync(string facultyUrl, int year);
    }
}
