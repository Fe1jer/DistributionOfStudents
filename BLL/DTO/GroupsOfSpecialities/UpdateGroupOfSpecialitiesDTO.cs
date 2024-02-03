using BLL.DTO.Base;
using BLL.DTO.Specialities;
using BLL.DTO.Subjects;

namespace BLL.DTO.GroupsOfSpecialities
{
    public class UpdateGroupOfSpecialitiesDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
        public FormOfEducationDTO FormOfEducation { get; set; } = new();
        public List<SelectedSubjectDTO>? Subjects { get; set; }
        public List<SelectedSpecialityDTO>? Specialities { get; set; }
    }
}
