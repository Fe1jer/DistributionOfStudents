using webapi.Data.Models;
using webapi.Data.Specifications.Base;
using System.Linq.Expressions;

namespace webapi.Data.Specifications
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
            AddInclude(f => f.Specialities);
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
