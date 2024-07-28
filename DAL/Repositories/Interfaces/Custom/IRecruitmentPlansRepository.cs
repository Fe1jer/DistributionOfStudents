using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IRecruitmentPlansRepository : IRepository<RecruitmentPlan>
    {
        Task<int> GetLastYearAsync();
        Task<RecruitmentPlan?> GetBySpecialityAndFormAsync(Guid specialityId, FormOfEducation form);
    }
}
