using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Specifications
{
    public class UsersSpecification : Specification<User>
    {
        public UsersSpecification() : base() { }

        public UsersSpecification WhereUserName(string username)
        {
            AddWhere(u => u.UserName == username);
            return this;
        }
    }
}
