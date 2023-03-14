using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Specifications
{
    public class FormOfEducationSpecification : Specification<FormOfEducation>
    {
        public FormOfEducationSpecification() { }

        public FormOfEducationSpecification WhereForm(FormOfEducation form)
        {
            AddWhere(p => p.IsFullTime == form.IsFullTime);
            AddWhere(p => p.IsDailyForm == form.IsDailyForm);
            AddWhere(p => p.IsBudget == form.IsBudget);
            AddWhere(p => p.Year == form.Year);

            return this;
        }
    }
}
