using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Postgres.Repositories.Custom
{
    public class RecruitmentPlansRepository : Repository<RecruitmentPlan>, IRecruitmentPlansRepository
    {
        public RecruitmentPlansRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<RecruitmentPlan?> GetBySpecialityAndFormAsync(Guid specialityId, FormOfEducation form)
        {
            return await EntitySet.SingleOrDefaultAsync(p => p.Speciality.Id == specialityId && p.FormOfEducation.Year == form.Year
            && p.FormOfEducation.IsDailyForm == form.IsDailyForm && p.FormOfEducation.IsBudget == form.IsBudget && p.FormOfEducation.IsFullTime == form.IsFullTime);
        }

        public async Task<int> GetLastYearAsync()
        {
            return await EntitySet.MaxAsync(p => p.FormOfEducation.Year);
        }
    }
}
