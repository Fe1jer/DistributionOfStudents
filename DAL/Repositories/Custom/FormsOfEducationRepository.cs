using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class FormsOfEducationRepository(ApplicationDbContext appDBContext) : Repository<FormOfEducation>(appDBContext), IFormsOfEducationRepository
    {
        public async Task<FormOfEducation?> GetByFormAsync(FormOfEducation form)
        {
            return await EntitySet.SingleOrDefaultAsync(p => p.Year == form.Year && p.IsDailyForm == form.IsDailyForm
                                                            && p.IsBudget == form.IsBudget && p.IsFullTime == form.IsFullTime);
        }
    }
}
