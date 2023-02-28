﻿using DistributionOfStudents.Data.Models;
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

        public GroupsOfSpecialitiesSpecification(int id) : this(faculty => faculty.Id == id) { }

        public GroupsOfSpecialitiesSpecification(Expression<Func<GroupOfSpecialties, bool>> expression) : base(expression)
        {
        }

        public GroupsOfSpecialitiesSpecification IncludeRecruitmentPlans()
        {
            AddInclude("Specialties.RecruitmentPlans");

            return this;
        }

        public GroupsOfSpecialitiesSpecification IncludeAdmissions()
        {
            AddInclude("Admissions.Student");
            AddInclude("Admissions.SpecialtyPriorities");
            AddInclude("Admissions.StudentScores");

            return this;
        }

        public GroupsOfSpecialitiesSpecification WhereFaculty(string facultyShortName)
        {
            AddWhere(p => p.Specialities.Any(i => i.Faculty.ShortName == facultyShortName));
            return this;
        }

        public GroupsOfSpecialitiesSpecification WhereCompleted()
        {
            AddWhere(p => p.IsCompleted == true);
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