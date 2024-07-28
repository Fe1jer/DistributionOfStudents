using DAL.Entities;

namespace BLL.DTO.RecruitmentPlans
{
    public class SpecialityPlansDTO
    {
        public SpecialityPlansDTO() { }
        public SpecialityPlansDTO(Speciality speciality)
        {
            speciality.RecruitmentPlans ??= new();
            FullName = speciality.DirectionName ?? speciality.FullName;
            SpecialityId = speciality.Id;
            SetCounts(speciality.RecruitmentPlans);
        }

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

        private void SetCounts(List<RecruitmentPlan> plans)
        {
            DailyFullBudget = GetCount(plans, true, true, true);
            DailyFullPaid = GetCount(plans, true, true, false);
            DailyAbbreviatedBudget = GetCount(plans, true, false, true);
            DailyAbbreviatedPaid = GetCount(plans, true, false, false);
            EveningFullBudget = GetCount(plans, false, true, true);
            EveningFullPaid = GetCount(plans, false, true, false);
            EveningAbbreviatedBudget = GetCount(plans, false, false, true);
            EveningAbbreviatedPaid = GetCount(plans, false, false, false);
        }

        private int GetCount(List<RecruitmentPlan> plans, bool isDailyForm, bool isFullTime, bool isBudget)
        {
            var matchingPlan = plans.FirstOrDefault(p =>
                p.FormOfEducation.IsDailyForm == isDailyForm &&
                p.FormOfEducation.IsFullTime == isFullTime &&
                p.FormOfEducation.IsBudget == isBudget);

            return matchingPlan?.Count ?? 0;
        }
    }

}
