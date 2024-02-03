using BLL.DTO.Base;
using BLL.DTO.RecruitmentPlans;

namespace BLL.DTO
{
    public class SpecialityPriorityDTO : EntityDTO
    {
        public RecruitmentPlanDTO RecruitmentPlan { get; set; } = new();
        public int Priority { get; set; }
    }
}