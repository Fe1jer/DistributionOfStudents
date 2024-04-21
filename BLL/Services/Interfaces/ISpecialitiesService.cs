using BLL.DTO.Specialities;

namespace BLL.Services.Interfaces
{
    public interface ISpecialitiesService
    {
        Task<List<SpecialityDTO>> GetAllAsync();
        Task<SpecialityDTO> GetAsync(Guid id);
        Task<SpecialityDTO> GetAsync(string url);
        Task<List<SpecialityDTO>> GetByGroupAsync(Guid groupId);
        Task<List<SpecialityDTO>> GetByFacultyAsync(string facultyUrl, bool isDisable = false);
        Task<bool> CheckUrlIsUniqueAsync(string url, Guid id);
        Task DeleteAsync(Guid id);
        Task<SpecialityDTO> SaveAsync(SpecialityDTO model);
    }
}
