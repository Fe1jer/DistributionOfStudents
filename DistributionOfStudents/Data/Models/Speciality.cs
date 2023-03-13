using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class Speciality : Entity
    {
        [Display(Name = "Название")]
        [DataType(DataType.Text)]
        public string FullName { get; set; } = String.Empty;

        [Display(Name = "Аббревиатура")]
        [DataType(DataType.Text)]
        public string? ShortName { get; set; }

        [Display(Name = "Код специальности")]
        public string Code { get; set; } = String.Empty;

        [Display(Name = "Сокращённый код специальности")]
        public string? ShortCode { get; set; }

        [Display(Name = "Описание специальности")]
        [DataType(DataType.Text)]
        public string? Description { get; set; }

        [Display(Name = "Направление")]
        [DataType(DataType.Text)]
        public string? DirectionName { get; set; }

        [Display(Name = "Код направления")]
        public string? DirectionCode { get; set; }

        [Display(Name = "Специализация")]
        [DataType(DataType.Text)]
        public string? SpecializationName { get; set; }

        [Display(Name = "Код специализации")]
        public string? SpecializationCode { get; set; }

        public Faculty Faculty { get; set; } = new();
        public List<GroupOfSpecialties>? GroupsOfSpecialties { get; set; }
        public List<RecruitmentPlan>? RecruitmentPlans { get; set; }
    }
}