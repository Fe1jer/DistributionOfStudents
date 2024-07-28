using BLL.DTO.Base;

namespace BLL.DTO.Students
{
    public class StudentItemDTO : EntityDTO
    {
        public string FacultyName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public Guid GroupOfSpecialtiesId { get; set; }
    }
}
