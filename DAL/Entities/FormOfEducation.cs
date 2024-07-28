using DAL.Entities.Base;

namespace DAL.Entities
{
    public class FormOfEducation : Entity
    {
        public int Year { get; set; }
        public bool IsDailyForm { get; set; }
        public bool IsBudget { get; set; }
        public bool IsFullTime { get; set; }
    }
}
