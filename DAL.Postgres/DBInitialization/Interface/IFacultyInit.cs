using DAL.Postgres.Entities;

namespace DAL.Postgres.DBInitialization.Interface
{
    public interface IFacultyInit
    {
        public Faculty GetFaculty();
        public List<Speciality> GetSpecialties();
        public List<GroupOfSpecialities> GetGroupsOfSpecialties(List<Speciality> specialities, FormOfEducation form);
        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form);
    }
}
