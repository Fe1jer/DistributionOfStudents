using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IStudentsRepository
    {
        Task<Student?> GetByIdAsync(int studentId);
        Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic);
        Task<List<Student>> GetAllAsync();
        Task<List<Student>> GetAllAsync(ISpecification<Student> specification);
        Task<List<Student>> SearchAsync(string searchText);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
    }
}
