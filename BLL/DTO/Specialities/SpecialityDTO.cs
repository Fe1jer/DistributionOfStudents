using BLL.DTO.Base;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;

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
        public FacultyDTO Faculty { get; set; } = new();
        public Guid FacultyId { get; set; }
        public List<GroupOfSpecialitiesDTO>? GroupsOfSpecialties { get; set; }
        public List<RecruitmentPlanDTO>? RecruitmentPlans { get; set; }
    }
}