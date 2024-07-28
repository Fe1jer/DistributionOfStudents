using DAL.Entities;
using DAL.Specifications.Base;

namespace DAL.Specifications
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
