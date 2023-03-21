using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;
using System.Linq.Expressions;

namespace DistributionOfStudents.Data.Specifications
{
    public class AdmissionsSpecification : Specification<Admission>
    {
        public AdmissionsSpecification() : base()
        {
            IncludeStudent();
        }

        public AdmissionsSpecification(Student student) : this(ad => ad.Student.Equals(student)) { }

        public AdmissionsSpecification(Expression<Func<Admission, bool>> expression) : base(expression)
        {
            IncludeStudent();
        }

        public AdmissionsSpecification IncludeGroup()
        {
            AddInclude(f => f.GroupOfSpecialties);

            return this;
        }

        public AdmissionsSpecification IncludeGroupWithSpecialities()
        {
            AddInclude("GroupOfSpecialties.Specialities.Faculty");

            return this;
        }

        private AdmissionsSpecification IncludeStudent()
        {
            AddInclude(f => f.Student);

            return this;
        }

        public AdmissionsSpecification IncludeSpecialtyPriorities()
        {
            AddInclude(f => f.SpecialityPriorities);
            AddInclude("SpecialityPriorities.RecruitmentPlan.Speciality");

            return this;
        }

        public AdmissionsSpecification IncludeStudentScores()
        {
            AddInclude("StudentScores.Subject");

            return this;
        }

        public AdmissionsSpecification SortByStudent()
        {
            AddOrdering(i => i.Student.Surname);
            AddOrdering(i => i.Student.Name);
            AddOrdering(i => i.Student.Patronymic);
            return this;
        }

        public AdmissionsSpecification SortByDate()
        {
            AddDescendingOrdering(f => f.DateOfApplication);
            return this;
        }

        public AdmissionsSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public AdmissionsSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
