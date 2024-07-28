using DAL.Entities.Base;

namespace DAL.Entities
{
    public class GroupOfSpecialitiesStatistic : Entity
    {
        public Guid GroupOfSpecialtiesId { get; set; }
        public GroupOfSpecialities GroupOfSpecialties { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.Today.ToUniversalTime();
        public int CountOfAdmissions { get; set; }
    }
}
