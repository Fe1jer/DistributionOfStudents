using BLL.DTO.Faculties;

namespace BLL.Services.Interfaces
{
    public interface IFacultiesService
    {
        Task<List<FacultyDTO>> GetAllAsync();
        Task<FacultyDTO> GetAsync(Guid id);
        Task<FacultyDTO> GetAsync(string url);
        Task<bool> CheckUrlIsUniqueAsync(string newUrl, string oldUrl);
        Task DeleteAsync(string url);
        Task<FacultyDTO> SaveAsync(FacultyDTO model);
    }
}
