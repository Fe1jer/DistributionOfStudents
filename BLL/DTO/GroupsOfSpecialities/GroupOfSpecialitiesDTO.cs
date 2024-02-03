using BLL.DTO.Base;
using BLL.DTO.Specialities;
using BLL.DTO.Subjects;

namespace BLL.DTO.GroupsOfSpecialities
{
    public class GroupOfSpecialitiesDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
        public FormOfEducationDTO FormOfEducation { get; set; } = new();
        public List<SubjectDTO>? Subjects { get; set; }
        public List<AdmissionDTO>? Admissions { get; set; }
        public List<SpecialityDTO>? Specialities { get; set; }
    }
}