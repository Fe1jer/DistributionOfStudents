using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;
using DAL.Postgres.Specifications.Base;
using Shared.Filters.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IAdmissionsRepository : IRepository<Admission>
    {
        Task<(List<Admission> rows, int count)> GetByFilterAsync(DefaultFilter filter, ISpecification<Admission> specification);
    }
}
