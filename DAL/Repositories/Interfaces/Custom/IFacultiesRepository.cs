using DAL.Entities;
using DAL.Repositories.Interfaces.Base;
using DAL.Specifications.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IFacultiesRepository : IRepository<Faculty>
    {
        Task<Faculty?> GetByUrlAsync(string url);
        Task<Faculty?> GetByUrlAsync(string url, ISpecification<Faculty> specification);
        Task<int> GetCountByUrlAsync(string newUrl, string oldUrl);
    }
}
