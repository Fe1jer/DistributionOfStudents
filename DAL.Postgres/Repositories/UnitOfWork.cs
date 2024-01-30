using DAL.Postgres.Context;
using DAL.Postgres.Repositories.Custom;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IAdmissionsRepository _admissions;
        private IFacultiesRepository _faculties;
        private IFormsOfEducationRepository _formsOfEducation;
        private ISpecialitiesRepository _specialities;
        private IGroupsOfSpecialitiesRepository _groupsOfSpecialities;
        private IGroupsOfSpecialitiesStatisticRepository _groupsOfSpecialitiesStatistic;
        private IRecruitmentPlansRepository _recruitmentPlans;
        private IRecruitmentPlansStatisticRepository _recruitmentPlansStatistic;
        private IStudentsRepository _students;
        private ISubjectsRepository _subjects;
        private IUserRepository _users;

        public UnitOfWork(ApplicationDbContext context) => _context = context;

        public IAdmissionsRepository Admissions => _admissions ??= new AdmissionsRepository(_context);
        public IFacultiesRepository Faculties => _faculties ??= new FacultiesRepository(_context);
        public IFormsOfEducationRepository FormsOfEducation => _formsOfEducation ??= new FormsOfEducationRepository(_context);
        public ISpecialitiesRepository Specialities => _specialities ??= new SpecialitiesRepository(_context);
        public IGroupsOfSpecialitiesRepository GroupsOfSpecialities => _groupsOfSpecialities ??= new GroupsOfSpecialitiesRepository(_context);
        public IGroupsOfSpecialitiesStatisticRepository IGroupsOfSpecialitiesStatistic => _groupsOfSpecialitiesStatistic ??= new GroupsOfSpecialitiesStatisticRepository(_context);
        public IRecruitmentPlansRepository RecruitmentPlans => _recruitmentPlans ??= new RecruitmentPlansRepository(_context);
        public IRecruitmentPlansStatisticRepository RecruitmentPlansStatistic => _recruitmentPlansStatistic ??= new RecruitmentPlansStatisticRepository(_context);
        public IStudentsRepository Students => _students ??= new StudentsRepository(_context);
        public ISubjectsRepository Subjects => _subjects ??= new SubjectsRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);

        public bool Commit()
        {
            return _context.SaveChanges() != 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
