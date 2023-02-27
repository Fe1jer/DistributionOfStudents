using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;
using System.Linq.Expressions;

namespace DistributionOfStudents.Data.Specifications
{
    public class SpecialitiesSpecification : Specification<Speciality>
    {

        public SpecialitiesSpecification() : base()
        {
        }

        public SpecialitiesSpecification(string shortName) : this(sp => sp.ShortName == shortName) { }

        public SpecialitiesSpecification(Expression<Func<Speciality, bool>> expression) : base(expression)
        {
        }

        public SpecialitiesSpecification SortBySpecialties()
        {
            AddOrdering(sp => int.Parse(string.Join("", sp.Code.Where(c => char.IsDigit(c)))));
            return this;
        }

        public SpecialitiesSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public SpecialitiesSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
