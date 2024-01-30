using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IAdmissionsRepository
    {
        Task<Admission?> GetByIdAsync(Guid admissionId);
        Task<Admission?> GetByIdAsync(Guid id, ISpecification<Admission> specification);
        Task<List<Admission>> GetAllAsync();
        Task<List<Admission>> GetAllAsync(ISpecification<Admission> specification);
        Task AddAsync(Admission admission);
        Task UpdateAsync(Admission admission);
        Task DeleteAsync(Guid id);
        Task<List<Admission>> SearchByStudentsAsync(string? searchText, ISpecification<Admission> specification);
    }
}
