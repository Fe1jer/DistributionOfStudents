using BLL.DTO.Base;
using BLL.DTO.GroupsOfSpecialities;

namespace BLL.DTO
{
    public class GroupOfSpecialitiesStatisticDTO : EntityDTO
    {
        public GroupOfSpecialitiesDTO GroupOfSpecialties { get; set; } = new();
        public DateTime Date { get; set; }
        public int CountOfAdmissions { get; set; }
    }
}
