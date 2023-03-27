using webapi.Data.Models;
using webapi.Data.Specifications;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IAdmissionsRepository
    {
        Task<Admission?> GetByIdAsync(int admissionId);
        Task<Admission?> GetByIdAsync(int id, ISpecification<Admission> specification);
        Task<List<Admission>> GetAllAsync();
        Task<List<Admission>> GetAllAsync(ISpecification<Admission> specification);
        Task AddAsync(Admission admission);
        Task UpdateAsync(Admission admission);
        Task DeleteAsync(int id);
        Task<List<Admission>> SearchByStudentsAsync(string? searchText, ISpecification<Admission> specification);
    }
}
