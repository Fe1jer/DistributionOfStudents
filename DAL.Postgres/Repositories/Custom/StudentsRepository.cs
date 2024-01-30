using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            Student? student = await GetByIdAsync(id);
            if (student != null)
            {
                await DeleteAsync(student);
            }
        }

        public async Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic)
        {
            var students = await GetAllAsync();
            return students.FirstOrDefault(st => st.Name == name && st.Surname == surname && st.Patronymic == patronymic);
        }

        public async Task<List<Student>> SearchAsync(string searchText)
        {
            var students = await GetAllAsync();
            if (searchText != null)
            {
                List<string> searchWords = searchText.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    students = students.Where(i => i.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(students.Where(i => i.Surname.ToLower().Contains(word.ToLower()))).Distinct()
                        .Union(students.Where(i => i.Patronymic.ToLower().Contains(word.ToLower()))).Distinct()
                        .ToList();
                }
            }

            return students;
        }
    }
}
