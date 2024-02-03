using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications.Base;
using Shared.Filters.Base;
using System.Linq;

namespace DAL.Postgres.Repositories.Custom
{
    public class AdmissionsRepository : Repository<Admission>, IAdmissionsRepository
    {
        public AdmissionsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            Admission? admission = await GetByIdAsync(id);
            if (admission != null)
            {
                await DeleteAsync(admission);
            }
        }

        public async Task<(List<Admission> rows, int count)> GetByFilterAsync(DefaultFilter filter, ISpecification<Admission> specification)
        {
            var admissions = await GetAllAsync(specification);
            int count = admissions.Count;
            if (filter.Search != null)
            {
                IEnumerable<string> searchWords = filter.Search.Split(" ");
                foreach (string word in searchWords)
                {
                    admissions = admissions.Where(i => string.Join(" ", i.Student.Name, i.Student.Surname, i.Student.Patronymic).Contains(word.ToLower())).ToList();
                }
            }

            return (admissions.Skip(filter.Skip).Take(filter.PageLimit).ToList(), count);
        }
    }
}
