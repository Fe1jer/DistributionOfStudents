using DAL.Entities.Base;

namespace DAL.Entities
{
    public class EnrolledStudent : Entity
    {
        public Student Student { get; set; } = null!;
    }
}