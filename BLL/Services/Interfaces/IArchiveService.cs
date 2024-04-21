using BLL.DTO.Faculties;

namespace BLL.Services.Interfaces
{
    public interface IArchiveService
    {
        Task<List<int>> GetYears();
        Task<List<string>> GetForms(int year);
        Task<List<ArchiveFacultyDTO>> GetFacultiesArchive(int year, string form);
    }
}
