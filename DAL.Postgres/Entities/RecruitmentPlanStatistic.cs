using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class RecruitmentPlanStatistic : Entity
    {
        public RecruitmentPlan RecruitmentPlan { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
