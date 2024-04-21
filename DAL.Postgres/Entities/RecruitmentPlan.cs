using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class RecruitmentPlan : Entity
    {
        public Speciality Speciality { get; set; } = null!;
        public Guid SpecialityId { get; set; }
        public int Count { get; set; }
        public int Target { get; set; }
        public int TargetPassingScore { get; set; }
        public int PassingScore { get; set; }
        public List<EnrolledStudent>? EnrolledStudents { get; set; }
        public FormOfEducation FormOfEducation { get; set; } = null!;
    }
}