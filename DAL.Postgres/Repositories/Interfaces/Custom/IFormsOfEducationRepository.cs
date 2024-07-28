using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IFormsOfEducationRepository : IRepository<FormOfEducation>
    {
        Task<FormOfEducation?> GetByFormAsync(FormOfEducation form);
    }
}
