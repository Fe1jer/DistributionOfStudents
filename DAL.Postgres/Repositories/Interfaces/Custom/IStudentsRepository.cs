using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IStudentsRepository : IRepository<Student>
    {
        Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic);
        Task<List<Student>> SearchAsync(string searchText);
    }
}
