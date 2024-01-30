using BLL.DTO.Base;

namespace BLL.DTO
{
    public class FacultyDTO : EntityDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = "\\img\\Faculties\\Default.jpg";
        public List<SpecialityDTO>? Specialities { get; set; }
    }
}