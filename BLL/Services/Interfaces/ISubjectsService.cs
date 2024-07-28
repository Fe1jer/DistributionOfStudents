using BLL.DTO.Subjects;

namespace BLL.Services.Interfaces
{
    public interface ISubjectsService
    {
        Task<List<SubjectDTO>> GetAllAsync();
        Task<List<SubjectDTO>> GetByGroupAsync(Guid groupId);
        Task<SubjectDTO> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<SubjectDTO> SaveAsync(SubjectDTO model);
    }
}
