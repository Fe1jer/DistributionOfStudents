using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
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
