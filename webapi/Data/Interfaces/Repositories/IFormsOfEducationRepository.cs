using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IFormsOfEducationRepository
    {
        Task<FormOfEducation?> GetByIdAsync(int formId);
        Task<List<FormOfEducation>> GetAllAsync();
        Task<List<FormOfEducation>> GetAllAsync(ISpecification<FormOfEducation> specification);
        Task AddAsync(FormOfEducation subject);
        Task UpdateAsync(FormOfEducation subject);
        Task DeleteAsync(int id);
    }
}
