using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Specifications
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
