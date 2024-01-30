using BLL.DTO.Base;

namespace BLL.DTO
{
    public class AdmissionDTO : EntityDTO
    {
        public StudentDTO Student { get; set; } = new();
        public GroupOfSpecialitiesDTO GroupOfSpecialties { get; set; } = new();
        public DateTime DateOfApplication { get; set; }
        public List<SpecialityPriorityDTO> SpecialityPriorities { get; set; } = new();
        public string? PassportID { get; set; }
        public string? PassportSeries { get; set; }
        public int? PassportNumber { get; set; }
        public bool IsTargeted { get; set; }
        public bool IsWithoutEntranceExams { get; set; }
        public bool IsOutOfCompetition { get; set; }
        public string? Email { get; set; }
        public List<StudentScoreDTO> StudentScores { get; set; } = new();
        public int Score
        {
            get
            {
                return StudentScores.Sum(i => i.Score) + Student.GPS;
            }
        }
    }
}
