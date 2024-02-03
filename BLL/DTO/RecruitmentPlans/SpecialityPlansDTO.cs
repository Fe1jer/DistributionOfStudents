using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.RecruitmentPlans
{
    public class SpecialityPlansDTO
    {
        public Guid SpecialityId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int DailyFullBudget { get; set; }
        public int DailyFullPaid { get; set; }
        public int DailyAbbreviatedBudget { get; set; }
        public int DailyAbbreviatedPaid { get; set; }
        public int EveningFullBudget { get; set; }
        public int EveningFullPaid { get; set; }
        public int EveningAbbreviatedBudget { get; set; }
        public int EveningAbbreviatedPaid { get; set; }
    }
}
