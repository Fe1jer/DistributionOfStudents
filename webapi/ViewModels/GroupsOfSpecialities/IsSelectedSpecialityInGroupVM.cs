﻿using webapi.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class IsSelectedSpecialityInGroupVM
    {
        public IsSelectedSpecialityInGroupVM() { }
        public IsSelectedSpecialityInGroupVM(Speciality speciality, bool isSelected)
        {
            SpecialityId = speciality.Id;
            SpecialityName = speciality.DirectionName ?? speciality.FullName;
            IsSelected = isSelected;
        }

        [Display(Name = "Специальность")]
        public string SpecialityName { get; set; } = string.Empty;

        public int SpecialityId { get; set; }

        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
