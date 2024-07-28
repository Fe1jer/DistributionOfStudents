using BLL.DTO.Base;
using BLL.DTO.Specialities;
using BLL.DTO.Students;

namespace BLL.DTO.RecruitmentPlans
{
    public class RecruitmentPlanDTO : EntityDTO
    {
        public SpecialityDTO Speciality { get; set; } = null!;
        public int Count { get; set; }
        public int Target { get; set; }
        public int TargetPassingScore { get; set; }
        public int PassingScore { get; set; }
        public List<EnrolledStudentDTO>? EnrolledStudents { get; set; }
        public FormOfEducationDTO FormOfEducation { get; set; } = null!;
    }
}