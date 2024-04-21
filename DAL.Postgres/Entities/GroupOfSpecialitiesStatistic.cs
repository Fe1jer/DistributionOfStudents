using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class GroupOfSpecialitiesStatistic : Entity
    {
        public GroupOfSpecialities GroupOfSpecialties { get; set; } = null!;
        public DateTime Date { get; set; }
        public int CountOfAdmissions { get; set; }
    }
}
