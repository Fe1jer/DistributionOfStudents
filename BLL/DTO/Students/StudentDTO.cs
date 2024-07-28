using BLL.DTO.Base;

namespace BLL.DTO.Students
{
    public class StudentDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public int GPA { get; set; }
    }
}
