using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Postgres.Repositories.Custom
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic)
        {
            return await EntitySet.FirstOrDefaultAsync(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                                            && i.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)
                                                            && i.Patronymic.Equals(patronymic, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<Student>> SearchAsync(string searchText)
        {
            var students = await GetAllAsync();
            if (searchText != null)
            {
                List<string> searchWords = searchText.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    students = students.Where(st => string.Join(" ", st.Name, st.Surname, st.Patronymic).Contains(word, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            return students;
        }
    }
}
