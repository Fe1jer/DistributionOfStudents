﻿using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Subject : Entity
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = String.Empty;

        public List<GroupOfSpecialties>? GroupsOfSpecialties { get; set; }
    }
}