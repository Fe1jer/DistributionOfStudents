using DAL.Postgres.Entities.Base;

namespace DAL.Postgres.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Patronymic { get; set; } = String.Empty;
        public string Role { get; set; } = String.Empty;
        public string Img { get; set; } = String.Empty;
    }
}
