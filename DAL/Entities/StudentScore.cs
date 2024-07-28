using DAL.Entities.Base;

namespace DAL.Entities
{
    public class StudentScore : Entity
    {
        public Subject Subject { get; set; } = null!;
        public Guid SubjectId { get; set; }

        public int Score { get; set; }
    }
}