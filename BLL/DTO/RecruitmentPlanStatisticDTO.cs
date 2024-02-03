using BLL.DTO.Base;
using BLL.DTO.RecruitmentPlans;

namespace BLL.DTO
{
    public class RecruitmentPlanStatisticDTO : EntityDTO
    {
        public RecruitmentPlanDTO RecruitmentPlan { get; set; } = new();
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
