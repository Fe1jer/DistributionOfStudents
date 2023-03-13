using DistributionOfStudents.Data.Models;

namespace DistributionOfStudents.Data.DBInitialization.Interface
{
    public interface IFacultyInit
    {
        public Faculty GetFaculty();
        public List<Speciality> GetSpecialties();
        public List<GroupOfSpecialties> GetGroupsOfSpecialties(List<Speciality> specialities);
        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities);
    }
}
