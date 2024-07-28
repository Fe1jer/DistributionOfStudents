using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class EnrolledStudent : Entity
    {
        public Student Student { get; set; } = null!;
    }
}