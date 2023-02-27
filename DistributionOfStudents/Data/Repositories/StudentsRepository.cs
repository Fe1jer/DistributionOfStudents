using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Student student = await GetByIdAsync(id);
            await DeleteAsync(student);
        }

        public async Task<Student> GetByFullNameAsync(string name, string surname, string patronymic)
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
