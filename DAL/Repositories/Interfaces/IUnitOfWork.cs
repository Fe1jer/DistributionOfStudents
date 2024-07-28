using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAdmissionsRepository Admissions { get; }
        IFacultiesRepository Faculties { get; }
        IFormsOfEducationRepository FormsOfEducation { get; }
        ISpecialitiesRepository Specialities { get; }
        IGroupsOfSpecialitiesRepository GroupsOfSpecialities { get; }
        IGroupsOfSpecialitiesStatisticRepository GroupsOfSpecialitiesStatistic { get; }
        IRecruitmentPlansRepository RecruitmentPlans { get; }
        IRecruitmentPlansStatisticRepository RecruitmentPlansStatistic { get; }
        IStudentsRepository Students { get; }
        ISubjectsRepository Subjects { get; }
        IUserRepository Users { get; }
        bool Commit();
    }
}
