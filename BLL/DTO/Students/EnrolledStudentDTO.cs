using BLL.DTO.Base;

namespace BLL.DTO.Students
{
    public class EnrolledStudentDTO : EntityDTO
    {
        public StudentDTO Student { get; set; } = null!;
        public List<StudentScoreDTO> StudentScores { get; set; } = new();
        public int Score => StudentScores.Sum(i => i.Score) + Student.GPA;
    }
}