using webapi.Data.Models;
using webapi.Data.Specifications.Base;
using System.Linq.Expressions;

namespace webapi.Data.Specifications
{
    public class RecruitmentPlansSpecification : Specification<RecruitmentPlan>
    {

        public RecruitmentPlansSpecification() : base()
        {
            IncludeFormOfEducation();
            SortBySpecialties();
        }

        public RecruitmentPlansSpecification(Expression<Func<RecruitmentPlan, bool>> expression) : base(expression)
        {
            IncludeFormOfEducation();
            SortBySpecialties();
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
            AddWhere(i => i.FormOfEducation.Id == group.FormOfEducation.Id);

            AddWhere(p => (group.Specialities ?? new()).Contains(p.Speciality));

            return this;
        }

        public RecruitmentPlansSpecification WhereForm(FormOfEducation form)
        {
            AddWhere(i => i.FormOfEducation.IsBudget == form.IsBudget &&
            i.FormOfEducation.IsDailyForm == form.IsDailyForm &&
            i.FormOfEducation.IsFullTime == form.IsFullTime &&
            i.FormOfEducation.Year == form.Year);

            return this;
        }

        public RecruitmentPlansSpecification WhereSpeciality(int specialityId)
        {
            AddWhere(i => i.Speciality.Id == specialityId);

            return this;
        }

        public RecruitmentPlansSpecification WhereFaculty(string facultyName)
        {
            AddWhere(i => i.Speciality.Faculty.ShortName == facultyName);

            return this;
        }

        public RecruitmentPlansSpecification WhereYear(int year)
        {
            AddWhere(i => i.FormOfEducation.Year == year);

            return this;
        }

        public RecruitmentPlansSpecification SortBySpecialties()
        {
            AddOrdering(f => f.Speciality.DirectionCode ?? f.Speciality.Code);
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

        private RecruitmentPlansSpecification IncludeFormOfEducation()
        {
            AddInclude(rp => rp.FormOfEducation);
            return this;
        }
    }
}
