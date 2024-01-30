using BLL.DTO.Base;

namespace BLL.DTO
{
    public class FormOfEducationDTO : EntityDTO
    {
        public int Year { get; set; }
        public bool IsDailyForm { get; set; }
        public bool IsBudget { get; set; }
        public bool IsFullTime { get; set; }
    }
}
