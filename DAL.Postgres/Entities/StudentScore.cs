using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class StudentScore : Entity
    {
        public Subject Subject { get; set; } = null!;
        public int Score { get; set; }
    }
}