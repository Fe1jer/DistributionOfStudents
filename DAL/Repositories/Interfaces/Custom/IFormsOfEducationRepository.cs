using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IFormsOfEducationRepository : IRepository<FormOfEducation>
    {
        Task<FormOfEducation?> GetByFormAsync(FormOfEducation form);
    }
}
