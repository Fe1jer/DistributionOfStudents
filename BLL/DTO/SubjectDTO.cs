using BLL.DTO.Base;

namespace BLL.DTO
{
    public class SubjectDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<GroupOfSpecialitiesDTO>? GroupsOfSpecialties { get; set; }
    }
}