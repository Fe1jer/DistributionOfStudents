using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class FormsOfEducationRepository : Repository<FormOfEducation>, IFormsOfEducationRepository
    {
        public FormsOfEducationRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<FormOfEducation?> GetByFormAsync(FormOfEducation form)
        {
            return await EntitySet.SingleOrDefaultAsync(p => p.Year == form.Year && p.IsDailyForm == form.IsDailyForm
                                                            && p.IsBudget == form.IsBudget && p.IsFullTime == form.IsFullTime);
        }
    }
}
