using webapi.Data.Models;

namespace webapi.Data.DBInitialization.Interface
{
    public interface IFacultyInit
    {
        public Faculty GetFaculty();
        public List<Speciality> GetSpecialties();
        public List<GroupOfSpecialties> GetGroupsOfSpecialties(List<Speciality> specialities, FormOfEducation form);
        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form);
    }
}
