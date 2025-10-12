using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class SubjectsRepository(ApplicationDbContext appDBContext) : Repository<Subject>(appDBContext), ISubjectsRepository
    {
    }
}
