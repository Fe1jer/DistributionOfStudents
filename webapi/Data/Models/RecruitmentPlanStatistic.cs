using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class RecruitmentPlanStatistic : Entity
    {
        public RecruitmentPlan RecruitmentPlan { get; set; } = new();

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Score { get; set; }
    }
}
