using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic)
        {
            return await EntitySet.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower()
                                                            && i.Surname.ToLower() == surname.ToLower()
                                                            && i.Patronymic.ToLower() == patronymic.ToLower());
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
