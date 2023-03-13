using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels.RecruitmentPlans
{
    public class PlansForSpecialityVM
    {
        public PlansForSpecialityVM() { }
        public PlansForSpecialityVM(Speciality speciality)
        {
            speciality.RecruitmentPlans ??= new();
            SpecialityName = speciality.DirectionName ?? speciality.FullName;
            SpecialityId = speciality.Id;
            DailyFullBudget = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && p.IsFullTime && p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => p.IsDailyForm && p.IsFullTime && p.IsBudget).Count : 0;
            DailyFullPaid = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && p.IsFullTime && !p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => p.IsDailyForm && p.IsFullTime && !p.IsBudget).Count : 0;
            DailyAbbreviatedBudget = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && !p.IsFullTime && p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => p.IsDailyForm && !p.IsFullTime && p.IsBudget).Count : 0;
            DailyAbbreviatedPaid = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && !p.IsFullTime && !p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => p.IsDailyForm && !p.IsFullTime && !p.IsBudget).Count : 0;
            EveningFullBudget = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && p.IsFullTime && p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => !p.IsDailyForm && p.IsFullTime && p.IsBudget).Count : 0;
            EveningFullPaid = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && p.IsFullTime && !p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => !p.IsDailyForm && p.IsFullTime && !p.IsBudget).Count : 0;
            EveningAbbreviatedBudget = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && !p.IsFullTime && p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => !p.IsDailyForm && !p.IsFullTime && p.IsBudget).Count : 0;
            EveningAbbreviatedPaid = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && !p.IsFullTime && !p.IsBudget) != null
            ? speciality.RecruitmentPlans.First(p => !p.IsDailyForm && !p.IsFullTime && !p.IsBudget).Count : 0;
        }

        [Display(Name = "Специальность")]
        public string SpecialityName { get; set; } = string.Empty;

        public int SpecialityId { get; set; }

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
