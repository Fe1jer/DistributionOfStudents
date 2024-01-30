﻿using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class Speciality : Entity
    {
        public string FullName { get; set; } = String.Empty;
        public string? ShortName { get; set; }
        public string Code { get; set; } = String.Empty;
        public string? ShortCode { get; set; }
        public string? Description { get; set; }
        public string? DirectionName { get; set; }
        public string? DirectionCode { get; set; }
        public string? SpecializationName { get; set; }
        public string? SpecializationCode { get; set; }
        public bool IsDisabled { get; set; }
        public Faculty Faculty { get; set; } = new();
        public List<GroupOfSpecialities>? GroupsOfSpecialties { get; set; }
        public List<RecruitmentPlan>? RecruitmentPlans { get; set; }
    }
}