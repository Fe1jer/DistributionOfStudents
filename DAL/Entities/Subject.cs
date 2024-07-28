using DAL.Entities.Base;

namespace DAL.Entities
{
    public class Subject : Entity
    {
        public string Name { get; set; } = string.Empty;
        public List<GroupOfSpecialities>? GroupsOfSpecialties { get; set; }
    }
}