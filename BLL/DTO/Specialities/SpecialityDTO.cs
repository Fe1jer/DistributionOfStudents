using BLL.DTO.Base;
using BLL.DTO.Faculties;

namespace BLL.DTO.Specialities
{
    public class SpecialityDTO : EntityDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? ShortCode { get; set; }
        public string? Description { get; set; }
        public string? DirectionName { get; set; }
        public string? DirectionCode { get; set; }
        public string? SpecializationName { get; set; }
        public string? SpecializationCode { get; set; }
        public bool IsDisabled { get; set; }
        public FacultyDTO? Faculty { get; set; }
        public Guid FacultyId { get; set; }
    }
}