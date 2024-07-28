using DAL.Entities.Base;

namespace DAL.Entities
{
    public class RecruitmentPlanStatistic : Entity
    {
        public Guid RecruitmentPlanId { get; set; }
        public RecruitmentPlan RecruitmentPlan { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.Today.ToUniversalTime();

        public int Score { get; set; }
    }
}
