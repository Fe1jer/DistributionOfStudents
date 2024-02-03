﻿using System.ComponentModel.DataAnnotations;
using webapi.Validations;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class UpdateGroupOfSpecialitiesViewModel : GroupOfSpecialitiesViewModel
    {
        public FormOfEducationViewModel FormOfEducation { get; set; } = new();

        [ValidateSelectedSpecialities]
        [Display(Name = "Специальности, составляющие общий конкурс")]
        public List<IsSelectedSpecialityInGroupVM> Specialities { get; set; } = new();

        [ValidateSelectedSubjects]
        [Display(Name = "Предметы, по которым нужны сертификаты")]
        public List<IsSelectedSubjectVM> Subjects { get; set; } = new();
    }
}
