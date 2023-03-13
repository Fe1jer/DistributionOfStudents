using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;
using System.Linq.Expressions;

namespace DistributionOfStudents.Data.Specifications
{
    public class GroupsOfSpecialitiesSpecification : Specification<GroupOfSpecialties>
    {
        public GroupsOfSpecialitiesSpecification(string facultyShortName) : base()
        {
            WhereFaculty(facultyShortName);
        }

        public GroupsOfSpecialitiesSpecification(Expression<Func<GroupOfSpecialties, bool>> expression) : base(expression)
        {
        }

        public GroupsOfSpecialitiesSpecification IncludeRecruitmentPlans()
        {
            AddInclude("Specialities.RecruitmentPlans");

            return this;
        }

        public GroupsOfSpecialitiesSpecification IncludeSubjects()
        {
#nullable disable
            AddInclude(f => f.Subjects);

#nullable restore
            return this;
        }

        public GroupsOfSpecialitiesSpecification IncludeSpecialties()
        {
#nullable disable
            AddInclude(gr => gr.Specialities);
#nullable restore

            return this;
        }

        public GroupsOfSpecialitiesSpecification IncludeAdmissions()
        {
            AddInclude("Admissions.Student");
            AddInclude("Admissions.SpecialityPriorities");
            AddInclude("Admissions.StudentScores.Subject");

            return this;
        }

        public GroupsOfSpecialitiesSpecification WhereFaculty(string facultyShortName)
        {
#nullable disable
            AddWhere(p => p.Specialities.Any(i => i.Faculty.ShortName == facultyShortName));
#nullable restore
            return this;
        }

        public GroupsOfSpecialitiesSpecification WhereCompleted()
        {
            AddWhere(p => p.IsCompleted == true);
            return this;
        }

        public GroupsOfSpecialitiesSpecification WhereYear(int year)
        {
            AddWhere(p => p.Year == year);
            return this;
        }

        public GroupsOfSpecialitiesSpecification SortByDate()
        {
            AddDescendingOrdering(f => f.StartDate);
            return this;
        }

        public GroupsOfSpecialitiesSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public GroupsOfSpecialitiesSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
