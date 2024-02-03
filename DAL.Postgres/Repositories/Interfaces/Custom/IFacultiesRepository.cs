using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IFacultiesRepository : IRepository<Faculty>
    {
        Task<Faculty?> GetByUrlAsync(string url);
        Task<Faculty?> GetByUrlAsync(string url, ISpecification<Faculty> specification);
        Task<int> GetCountByUrlAsync(string url, Guid excludeId);
    }
}
