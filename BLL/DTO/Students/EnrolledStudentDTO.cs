using BLL.DTO.Base;

namespace BLL.DTO.Students
{
    public class EnrolledStudentDTO : EntityDTO
    {
        public StudentDTO Student { get; set; } = null!;
    }
}