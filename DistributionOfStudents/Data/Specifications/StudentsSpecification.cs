﻿using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;
using System.Linq.Expressions;

namespace DistributionOfStudents.Data.Specifications
{
    public class StudentsSpecification : Specification<Student>
    {

        public StudentsSpecification() : base()
        {
            SortBySurname();
        }

        public StudentsSpecification(Expression<Func<Student, bool>> expression) : base(expression)
        {
            SortBySurname();
        }

        private StudentsSpecification SortBySurname()
        {
            AddOrdering(st => st.Surname);
            AddOrdering(st => st.Name);
            AddOrdering(st => st.Patronymic);

            return this;
        }

        public StudentsSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public StudentsSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
