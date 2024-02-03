using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;
using System.Linq.Expressions;

namespace DAL.Postgres.Specifications
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

        public SpecialitiesSpecification SortByCode()
        {
            AddOrdering(sp => sp.DirectionCode ?? sp.Code);
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

        public SpecialitiesSpecification IncludeRecruitmentPlans()
        {
            AddInclude("RecruitmentPlans.FormOfEducation");

            return this;
        }
    }
}
