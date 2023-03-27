using webapi.Data.AbstractClasses;

namespace webapi.Data.Models
{
    public class EnrolledStudent : Entity
    {
        public Student Student { get; set; } = new();
    }
}