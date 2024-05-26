using BLL.DTO;
using BLL.DTO.Students;
using Shared.Filters.Base;

namespace BLL.Services.Interfaces
{
    public interface IAdmissionsService
    {
        Task<List<AdmissionDTO>> GetAllAsync();
        Task<(List<StudentItemDTO> rows, int count)> GetByLastYearAsync(DefaultFilter filter);
        Task<(List<AdmissionDTO> rows, int count)> GetByGroupAsync(Guid groupId, DefaultFilter filter);
        Task<List<AdmissionDTO>> GetByGroupAsync(Guid groupId);
        Task<AdmissionDTO> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<AdmissionDTO> SaveAsync(AdmissionDTO model);
    }
}
