using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;
using System.Linq.Expressions;

namespace DAL.Postgres.Specifications
{
    public class SpecialitiesSpecification : Specification<Speciality>
    {

        public SpecialitiesSpecification()
        {
        }

        public SpecialitiesSpecification(string shortName) : this(sp => sp.ShortName == shortName) { }

        public SpecialitiesSpecification(Expression<Func<Speciality, bool>> expression) : base(expression)
        {
        }

        public SpecialitiesSpecification WhereFaculty(string facultyUrl)
        {
            AddWhere(i => i.Faculty.ShortName == facultyUrl);

            return this;
        }

        public SpecialitiesSpecification SortBySpecialties()
        {
            AddOrdering(sp => int.Parse(string.Join("", sp.Code.Where(char.IsDigit))));
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

        public SpecialitiesSpecification IncludeRecruitmentPlans(int year)
        {
            AddInclude(i => i.RecruitmentPlans.Where(p => p.FormOfEducation.Year == year));
            AddInclude("RecruitmentPlans.FormOfEducation");

            return this;
        }
    }
}
