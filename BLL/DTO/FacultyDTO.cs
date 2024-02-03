using BLL.DTO.Base;
using BLL.DTO.Specialities;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters;

namespace BLL.DTO
{
    public class FacultyDTO : EntityDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = "\\img\\Faculties\\Default.jpg";
        public List<SpecialityDTO>? Specialities { get; set; }
        public IFormFile? FileImg { get; set; }
    }
}