using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class Admission : Entity
    {
        public Student Student { get; set; } = null!;
        public GroupOfSpecialities GroupOfSpecialties { get; set; } = null!;
        public Guid GroupOfSpecialtiesId { get; set; }
        public DateTime DateOfApplication { get; set; }
        public List<SpecialityPriority> SpecialityPriorities { get; set; } = new();
        public string? PassportID { get; set; }
        public string? PassportSeries { get; set; }
        public int? PassportNumber { get; set; }
        public bool IsTargeted { get; set; }
        public bool IsWithoutEntranceExams { get; set; }
        public bool IsOutOfCompetition { get; set; }
        public string? Email { get; set; }
        public List<StudentScore> StudentScores { get; set; } = new();
        public int Score
        {
            get
            {
                return StudentScores.Sum(i => i.Score) + Student.GPA;
            }
        }
    }
}
