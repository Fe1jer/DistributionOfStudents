using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;
using System.Linq.Expressions;

namespace DAL.Postgres.Specifications
{
    public class FacultiesSpecification : Specification<Faculty>
    {
        public FacultiesSpecification() : base()
        {
        }

        public FacultiesSpecification(string shortName) : this(faculty => faculty.ShortName.ToLower().Contains(shortName.ToLower())) { }

        public FacultiesSpecification(Expression<Func<Faculty, bool>> expression) : base(expression)
        {
        }

        public FacultiesSpecification IncludeSpecialties()
        {
#nullable disable
            AddInclude(f => f.Specialities.Where(s => !s.IsDisabled).OrderBy(s => s.DirectionCode ?? s.Code));
#nullable restore

            return this;
        }

        public FacultiesSpecification IncludeDisabledSpecialties()
        {
#nullable disable
            AddInclude(f => f.Specialities.Where(s => s.IsDisabled).OrderBy(s => s.DirectionCode ?? s.Code));
#nullable restore

            return this;
        }

        public FacultiesSpecification IncludeEnrolledStudents()
        {
            AddInclude("Specialities.RecruitmentPlans.EnrolledStudents");

            return this;
        }

        public FacultiesSpecification IncludeRecruitmentPlans()
        {
            AddInclude("Specialities.RecruitmentPlans.FormOfEducation");

            return this;
        }

        public FacultiesSpecification WhereShortName(string name)
        {
            AddWhere(f => f.ShortName == name);
            return this;
        }

        public FacultiesSpecification SortByDate()
        {
            AddDescendingOrdering(f => f.ShortName);
            return this;
        }

        public FacultiesSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public FacultiesSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
