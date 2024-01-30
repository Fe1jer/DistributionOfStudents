using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class Student : Entity
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Patronymic { get; set; } = String.Empty;
        public int GPS { get; set; }
        public List<Admission>? Admissions { get; set; }
    }
}
