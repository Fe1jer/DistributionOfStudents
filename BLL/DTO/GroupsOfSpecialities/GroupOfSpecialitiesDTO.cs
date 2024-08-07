﻿using BLL.DTO.Base;

namespace BLL.DTO.GroupsOfSpecialities
{
    public class GroupOfSpecialitiesDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
        public FormOfEducationDTO FormOfEducation { get; set; } = null!;
    }
}