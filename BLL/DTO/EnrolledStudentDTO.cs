using BLL.DTO.Base;

namespace BLL.DTO
{
    public class EnrolledStudentDTO : EntityDTO
    {
        public StudentDTO Student { get; set; } = new();
    }
}