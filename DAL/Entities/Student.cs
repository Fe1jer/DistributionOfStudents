using DAL.Entities.Base;

namespace DAL.Entities
{
    public class Student : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public int GPA { get; set; }
        public List<Admission>? Admissions { get; set; }
    }
}
