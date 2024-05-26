using BLL.DTO.Base;
using BLL.DTO.RecruitmentPlans;

namespace BLL.DTO
{
    public class SpecialityPriorityDTO : EntityDTO
    {
        public RecruitmentPlanDTO RecruitmentPlan { get; set; } = null!;
        public Guid RecruitmentPlanId { get; set; }
        public int Priority { get; set; }
    }
}