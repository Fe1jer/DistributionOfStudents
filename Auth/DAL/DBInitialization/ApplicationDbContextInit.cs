using DAL.Context;
using DAL.Entities;

namespace DAL.DBInitialization
{
    public class AuthDbContextInit
    {
        public static void InitDbContext(AuthDbContext? context)
        {
            if (context != null)
            {
                if (!context.Users.Any())
                {
                    CreateUsers(context);
                }
            }
        }

        private static void CreateUsers(AuthDbContext context)
        {
            User admin = new()
            {
                UserName = "admin@gmail.com",
                Name = "Admin",
                Surname = "Admin",
                Patronymic = "Admin",
                Role = "admin",
                //adminDistribution
                PasswordHash = "APlb1LzG7drutYVMOyH/efSmFw0fKRnU6R4hBXzi6fAeN2k99IsepVTCrPZvtMt3mg==",
                Img = "\\img\\Users\\bntu.jpg"
            };
            context.Users.Add(admin);
            context.SaveChanges();

            User commission = new()
            {
                UserName = "commission@gmail.com",
                Name = "commission",
                Surname = "commission",
                Patronymic = "commission",
                Role = "commission",
                //commissionDistribution
                PasswordHash = "AH1/j12C/ytndGn2av/Mp8NJGkPHJygvpF8mMBNnR9E7xUWCcNPuKCBH7cnlOMIPIA==",
                Img = "\\img\\Users\\bntu.jpg"
            };
            context.Users.Add(commission);
            context.SaveChanges();
        }
    }
}
