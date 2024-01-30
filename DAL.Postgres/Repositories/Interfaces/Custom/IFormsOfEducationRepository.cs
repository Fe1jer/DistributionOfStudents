using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IFormsOfEducationRepository
    {
        Task<FormOfEducation?> GetByIdAsync(Guid formId);
        Task<List<FormOfEducation>> GetAllAsync();
        Task<List<FormOfEducation>> GetAllAsync(ISpecification<FormOfEducation> specification);
        Task AddAsync(FormOfEducation subject);
        Task UpdateAsync(FormOfEducation subject);
        Task DeleteAsync(Guid id);
    }
}
