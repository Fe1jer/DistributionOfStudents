using BLL.DTO.Specialities;

namespace BLL.DTO.GroupsOfSpecialities
{
    public class ArchiveGroupOfSpecialitiesDTO
    {
        public List<ArchiveSpecialityPlanDTO> SpetialityPlans { get; set; } = new();
        public float Competition { get; set; }
    }
}
