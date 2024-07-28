using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class StudentScore : Entity
    {
        public Subject Subject { get; set; } = null!;
        public Guid SubjectId { get; set; }

        public int Score { get; set; }
    }
}