using BLL.DTO.Base;

namespace webapi.ViewModels.Students
{
    public class EnrolledStudentViewModel : EntityDTO
    {
        public StudentViewModel Student { get; set; } = null!;
    }
}