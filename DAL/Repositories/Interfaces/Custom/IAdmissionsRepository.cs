using DAL.Entities;
using DAL.Repositories.Interfaces.Base;
using DAL.Specifications.Base;
using Shared.Filters.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IAdmissionsRepository : IRepository<Admission>
    {
        Task<(List<Admission> rows, int count)> GetByFilterAsync(DefaultFilter filter, ISpecification<Admission> specification);
    }
}
