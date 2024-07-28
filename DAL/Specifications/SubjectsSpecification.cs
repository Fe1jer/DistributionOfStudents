using DAL.Entities;
using DAL.Specifications.Base;
using System.Linq.Expressions;

namespace DAL.Specifications
{
    public class SubjectsSpecification : Specification<Subject>
    {
        public SubjectsSpecification() : base()
        {
        }

        public SubjectsSpecification(Expression<Func<Subject, bool>> expression) : base(expression)
        {
        }

        public SubjectsSpecification SortByName()
        {
            AddOrdering(st => st.Name);

            return this;
        }

        public SubjectsSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public SubjectsSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
