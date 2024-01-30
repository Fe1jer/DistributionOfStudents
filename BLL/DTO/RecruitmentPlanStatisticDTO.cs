using BLL.DTO.Base;

namespace BLL.DTO
{
    public class RecruitmentPlanStatisticDTO : EntityDTO
    {
        public RecruitmentPlanDTO RecruitmentPlan { get; set; } = new();
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
