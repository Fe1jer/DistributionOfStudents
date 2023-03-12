using DistributionOfStudents.Data.AbstractClasses;

namespace DistributionOfStudents.Data.Models
{
    public class EnrolledStudent : Entity
    {
        public Student Student { get; set; } = new();
    }
}