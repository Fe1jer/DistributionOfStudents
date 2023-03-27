using webapi.Data.AbstractClasses;

namespace webapi.Data.Models
{
    public class SpecialityPriority : Entity
    {
        public RecruitmentPlan RecruitmentPlan { get; set; } = new();
        public int Priority { get; set; }
    }
}