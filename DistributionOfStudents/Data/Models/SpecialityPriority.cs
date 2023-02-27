using DistributionOfStudents.Data.AbstractClasses;

namespace DistributionOfStudents.Data.Models
{
    public class SpecialityPriority : Entity
    {
        public RecruitmentPlan RecruitmentPlan { get; set; }
        public int Priority { get; set; }
    }
}