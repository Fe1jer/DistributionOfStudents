using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class RecruitmentPlansRepository(ApplicationDbContext appDBContext) : Repository<RecruitmentPlan>(appDBContext), IRecruitmentPlansRepository
    {
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
