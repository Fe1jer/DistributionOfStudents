using DAL.Entities;
using DAL.Repositories.Interfaces.Base;
using DAL.Specifications.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface ISpecialitiesRepository : IRepository<Speciality>
    {
        Task<Speciality?> GetByUrlAsync(string url);
        Task<List<Speciality>> GetByFacultyAsync(string facultyUrl, bool isDisable);
        Task<Speciality?> GetByUrlAsync(string url, ISpecification<Speciality> specification);
        Task<int> GetCountByUrlAsync(string url, Guid excludeId);
    }
}
