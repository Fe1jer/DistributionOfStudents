using DAL.Entities;
using DAL.Specifications.Base;

namespace DAL.Specifications
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
