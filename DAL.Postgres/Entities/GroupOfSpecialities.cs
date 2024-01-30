using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class GroupOfSpecialities : Entity
    {
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
        public FormOfEducation FormOfEducation { get; set; } = new();
        public List<Subject>? Subjects { get; set; }
        public List<Admission>? Admissions { get; set; }
        public List<Speciality>? Specialities { get; set; }
    }
}