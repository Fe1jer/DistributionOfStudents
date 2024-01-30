using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications.Base;

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

        public async Task<List<Admission>> SearchByStudentsAsync(string? searchText, ISpecification<Admission> specification)
        {
            var admissions = await GetAllAsync(specification);
            if (searchText != null)
            {
                List<string> searchWords = searchText.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    admissions = admissions.Where(i => i.Student.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(admissions.Where(i => i.Student.Surname.ToLower().Contains(word.ToLower()))).Distinct()
                        .Union(admissions.Where(i => i.Student.Patronymic.ToLower().Contains(word.ToLower()))).Distinct()
                        .ToList();
                }
            }

            return admissions;
        }
    }
}
