using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DAL.Postgres.Repositories.Custom
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
