using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;
using System.Linq.Expressions;

namespace DistributionOfStudents.Data.Specifications
{
    public class RecruitmentPlansSpecification : Specification<RecruitmentPlan>
    {

        public RecruitmentPlansSpecification() : base()
        {
        }

        public RecruitmentPlansSpecification(Expression<Func<RecruitmentPlan, bool>> expression) : base(expression)
        {
        }

        public RecruitmentPlansSpecification IncludeSpecialty()
        {
            AddInclude(rp => rp.Speciality);

            return this;
        }

        public RecruitmentPlansSpecification IncludeEnrolledStudents()
        {
            AddInclude("EnrolledStudents.Student");

            return this;
        }

        public RecruitmentPlansSpecification WhereGroup(GroupOfSpecialties group)
        {
            AddWhere(i => i.IsBudget == group.IsBudget && i.IsDailyForm == group.IsDailyForm && i.IsFullTime == group.IsFullTime && i.Year == group.Year);

            return this;
        }

        public RecruitmentPlansSpecification WhereForm(bool isDailyForm, bool isFullTime, bool isBudget, int year, int specialityId)
        {
            AddWhere(i => i.IsBudget == isBudget && i.IsDailyForm == isDailyForm && i.IsFullTime == isFullTime && i.Year == year && i.Speciality.Id == specialityId);

            return this;
        }

        public RecruitmentPlansSpecification WhereFaculty(string facultyName)
        {
            AddWhere(i => i.Speciality.Faculty.ShortName == facultyName);

            return this;
        }

        public RecruitmentPlansSpecification WhereYear(int year)
        {
            AddWhere(i => i.Year == year);

            return this;
        }

        public RecruitmentPlansSpecification SortBySpecialties()
        {
            AddOrdering(f => int.Parse(string.Join("", f.Speciality.Code.Where(c => char.IsDigit(c)))));
            return this;
        }

        public RecruitmentPlansSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public RecruitmentPlansSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
