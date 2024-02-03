using BLL.DTO.GroupsOfSpecialities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IGroupsOfSpecialitiesService
    {
        Task<List<GroupOfSpecialitiesDTO>> GetAllAsync();
        Task<GroupOfSpecialitiesDTO> GetAsync(Guid id);
        Task<List<GroupOfSpecialitiesDTO>> GetByFacultyAsync(string facultyUrl, int year);
        Task DeleteAsync(Guid id);
        Task<GroupOfSpecialitiesDTO> SaveAsync(UpdateGroupOfSpecialitiesDTO model);
    }
}
