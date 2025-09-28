namespace BLL.Services.Interfaces
{
    public interface ICryptographyService
    {
        public string GetHashPassword(string password);
        public bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
