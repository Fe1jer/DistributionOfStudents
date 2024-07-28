using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IRecruitmentPlansRepository : IRepository<RecruitmentPlan>
    {
        Task<int> GetLastYearAsync();
        Task<RecruitmentPlan?> GetBySpecialityAndFormAsync(Guid specialityId, FormOfEducation form);
    }
}
