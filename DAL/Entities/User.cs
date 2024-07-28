using DAL.Entities.Base;

namespace DAL.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Img { get; set; } = "\\img\\Users\\bntu.jpg";
    }
}
