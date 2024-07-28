using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IStudentsRepository : IRepository<Student>
    {
        Task<Student?> GetByFullNameAsync(string name, string surname, string patronymic);
        Task<List<Student>> SearchAsync(string searchText);
    }
}
