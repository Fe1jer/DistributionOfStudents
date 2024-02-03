using System.ComponentModel.DataAnnotations;
using webapi.Data.Models;

namespace webapi.ViewModels.RecruitmentPlans
{
    public class SpecialityPlansViewModel
    {
        public SpecialityPlansViewModel() { }
        public SpecialityPlansViewModel(Speciality speciality)
        {
            speciality.RecruitmentPlans ??= new();
            FullName = speciality.DirectionName ?? speciality.FullName;
            SpecialityId = speciality.Id;
            DailyFullBudget = speciality.RecruitmentPlans.FirstOrDefault(p => p.FormOfEducation.IsDailyForm && p.FormOfEducation.IsFullTime && p.FormOfEducation.IsBudget) ?? 0;
            DailyFullPaid = speciality.RecruitmentPlans.FirstOrDefault(p => p.FormOfEducation.IsDailyForm && p.FormOfEducation.IsFullTime && !p.FormOfEducation.IsBudget) ?? 0;
            DailyAbbreviatedBudget = speciality.RecruitmentPlans.FirstOrDefault(p => p.FormOfEducation.IsDailyForm && !p.FormOfEducation.IsFullTime && p.FormOfEducation.IsBudget) ?? 0;
            DailyAbbreviatedPaid = speciality.RecruitmentPlans.FirstOrDefault(p => p.FormOfEducation.IsDailyForm && !p.FormOfEducation.IsFullTime && !p.FormOfEducation.IsBudget) ?? 0;
            EveningFullBudget = speciality.RecruitmentPlans.FirstOrDefault(p => !p.FormOfEducation.IsDailyForm && p.FormOfEducation.IsFullTime && p.FormOfEducation.IsBudget) ?? 0;
            EveningFullPaid = speciality.RecruitmentPlans.FirstOrDefault(p => !p.FormOfEducation.IsDailyForm && p.FormOfEducation.IsFullTime && !p.FormOfEducation.IsBudget) ?? 0;
            EveningAbbreviatedBudget = speciality.RecruitmentPlans.FirstOrDefault(p => !p.FormOfEducation.IsDailyForm && !p.FormOfEducation.IsFullTime && p.FormOfEducation.IsBudget) ?? 0;
            EveningAbbreviatedPaid = speciality.RecruitmentPlans.FirstOrDefault(p => !p.FormOfEducation.IsDailyForm && !p.FormOfEducation.IsFullTime && !p.FormOfEducation.IsBudget) ?? 0;
        }
        public int SpecialityId { get; set; }

        [Display(Name = "Специальность (направление специальности)")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Бюджет")]
        public int DailyFullBudget { get; set; }

        [Display(Name = "Платное")]
        public int DailyFullPaid { get; set; }

        [Display(Name = "Бюджет сокр.срок")]
        public int DailyAbbreviatedBudget { get; set; }

        [Display(Name = "Платное сокр.срок")]
        public int DailyAbbreviatedPaid { get; set; }

        [Display(Name = "Бюджет")]
        public int EveningFullBudget { get; set; }

        [Display(Name = "Платное")]
        public int EveningFullPaid { get; set; }

        [Display(Name = "Бюджет сокр.срок")]
        public int EveningAbbreviatedBudget { get; set; }

        [Display(Name = "Платное сокр.срок")]
        public int EveningAbbreviatedPaid { get; set; }
    }
}
