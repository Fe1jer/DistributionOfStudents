using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IFacultiesService
    {
        Task<List<FacultyDTO>> GetAllAsync();
        Task<FacultyDTO> GetAsync(Guid id);
        Task<FacultyDTO> GetAsync(string url);
        Task<bool> CheckUrlIsUniqueAsync(string url, Guid id);
        Task DeleteAsync(Guid id);
        Task<FacultyDTO> SaveAsync(FacultyDTO model);
    }
}
