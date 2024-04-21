using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;
using System.Linq.Expressions;

namespace DAL.Postgres.Specifications
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

        public RecruitmentPlansSpecification IncludeFaculty()
        {
            AddInclude(rp => rp.Speciality.Faculty);

            return this;
        }

        public RecruitmentPlansSpecification IncludeEnrolledStudents()
        {
            AddInclude("EnrolledStudents.Student");

            return this;
        }

        public RecruitmentPlansSpecification WhereGroup(GroupOfSpecialities group)
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

        public RecruitmentPlansSpecification WhereSpeciality(Guid specialityId)
        {
            AddWhere(i => i.Speciality.Id == specialityId);

            return this;
        }

        public RecruitmentPlansSpecification WhereFaculty(string facultyUrl)
        {
            AddWhere(i => i.Speciality.Faculty.ShortName == facultyUrl);

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
