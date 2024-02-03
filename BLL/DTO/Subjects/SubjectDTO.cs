using BLL.DTO.Base;
using BLL.DTO.GroupsOfSpecialities;

namespace BLL.DTO.Subjects
{
    public class SubjectDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<GroupOfSpecialitiesDTO>? GroupsOfSpecialties { get; set; }
    }
}