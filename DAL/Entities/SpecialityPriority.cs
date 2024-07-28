using DAL.Entities.Base;

namespace DAL.Entities
{
    public class SpecialityPriority : Entity
    {
        public RecruitmentPlan RecruitmentPlan { get; set; } = null!;
        public Guid RecruitmentPlanId { get; set; }
        public int Priority { get; set; }
    }
}