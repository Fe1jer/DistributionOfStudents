using BLL.DTO.Base;

namespace BLL.DTO
{
    public class RecruitmentPlanDTO : EntityDTO
    {
        public SpecialityDTO Speciality { get; set; } = new();
        public int Count { get; set; }
        public int Target { get; set; }
        public int TargetPassingScore { get; set; }
        public int PassingScore { get; set; }
        public List<EnrolledStudentDTO>? EnrolledStudents { get; set; }
        public FormOfEducationDTO FormOfEducation { get; set; } = new();
    }
}