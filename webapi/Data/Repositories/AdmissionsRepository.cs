using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;
using webapi.Data.Specifications;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Repositories
{
    public class AdmissionsRepository : Repository<Admission>, IAdmissionsRepository
    {
        public AdmissionsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
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
