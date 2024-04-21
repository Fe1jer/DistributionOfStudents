using Microsoft.AspNetCore.Http;

namespace BLL.DTO.User
{
    public class RegisterDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public IFormFile? FileImg { get; set; }
    }
}
