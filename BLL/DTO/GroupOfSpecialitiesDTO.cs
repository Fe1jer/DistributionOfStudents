﻿using BLL.DTO.Base;

namespace BLL.DTO
{
    public class GroupOfSpecialitiesDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
        public FormOfEducationDTO FormOfEducation { get; set; } = new();
        public List<SubjectDTO>? Subjects { get; set; }
        public List<AdmissionDTO>? Admissions { get; set; }
        public List<SpecialityDTO>? Specialities { get; set; }
    }
}