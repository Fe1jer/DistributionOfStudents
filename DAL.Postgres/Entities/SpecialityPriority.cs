using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class SpecialityPriority : Entity
    {
        public RecruitmentPlan RecruitmentPlan { get; set; } = null!;
        public int Priority { get; set; }
    }
}