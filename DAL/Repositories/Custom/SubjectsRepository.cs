using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories.Custom
{
    public class SubjectsRepository(ApplicationDbContext appDBContext) : Repository<Subject>(appDBContext), ISubjectsRepository
    {
    }
}
