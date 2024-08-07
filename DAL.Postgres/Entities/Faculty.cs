﻿using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class Faculty : Entity
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = "\\img\\Faculties\\Default.jpg";
        public List<Speciality>? Specialities { get; set; }
    }
}