using DAL.Entities;

namespace DAL.DBInitialization.Interface
{
    public interface IFacultyInit
    {
        public Faculty GetFaculty();
        public List<Speciality> GetSpecialties();
        public List<GroupOfSpecialities> GetGroupsOfSpecialties(List<Speciality> specialities,List<Subject> subjects, FormOfEducation form);
        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form);
    }
}
