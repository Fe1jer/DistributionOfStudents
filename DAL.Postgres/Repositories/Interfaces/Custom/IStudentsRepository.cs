using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IStudentsRepository
    {
        Task<Student?> GetByIdAsync(Guid studentId);
        Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic);
        Task<List<Student>> GetAllAsync();
        Task<List<Student>> GetAllAsync(ISpecification<Student> specification);
        Task<List<Student>> SearchAsync(string searchText);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Guid id);
    }
}
