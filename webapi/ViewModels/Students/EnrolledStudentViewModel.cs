using BLL.DTO.Base;
using webapi.ViewModels.Admissions;

namespace webapi.ViewModels.Students
{
    public class EnrolledStudentViewModel : EntityDTO
    {
        public StudentViewModel Student { get; set; } = null!;
        public List<StudentScoreViewModel> StudentScores { get; set; } = new();
        public int Score => StudentScores.Sum(i => i.Score) + Student.GPA;
    }
}