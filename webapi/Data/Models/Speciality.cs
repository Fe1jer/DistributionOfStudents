﻿using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Speciality : Entity
    {
        [Display(Name = "Специальность")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string FullName { get; set; } = String.Empty;

        [Display(Name = "Аббревиатура")]
        [DataType(DataType.Text)]
        public string? ShortName { get; set; }

        [Display(Name = "Код специальности")]
        [Required(ErrorMessage = "Введите код")]
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

        public bool IsDisabled { get; set; }

        public Faculty Faculty { get; set; } = new();
        public List<GroupOfSpecialties>? GroupsOfSpecialties { get; set; }
        public List<RecruitmentPlan>? RecruitmentPlans { get; set; }
    }
}