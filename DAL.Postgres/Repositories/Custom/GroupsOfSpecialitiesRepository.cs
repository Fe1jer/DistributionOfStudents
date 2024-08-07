﻿using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications;

namespace DAL.Postgres.Repositories.Custom
{
    public class GroupsOfSpecialitiesRepository : Repository<GroupOfSpecialities>, IGroupsOfSpecialitiesRepository
    {
        public GroupsOfSpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public Task<List<GroupOfSpecialities>> GetByFacultyAsync(string facultyUrl, int year)
        {
            return GetAllAsync(new GroupsOfSpecialitiesSpecification().WhereFaculty(facultyUrl).WhereYear(year));
        }

        public async Task DeleteAsync(Guid id)
        {
            GroupOfSpecialities? GroupOfSpecialities = await GetByIdAsync(id);
            if (GroupOfSpecialities != null)
            {
                await DeleteAsync(GroupOfSpecialities);
            }
        }
    }
}
