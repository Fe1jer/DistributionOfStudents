using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class Subject : Entity
    {
        public string Name { get; set; } = String.Empty;
        public List<GroupOfSpecialities>? GroupsOfSpecialties { get; set; }
    }
}