using BLL.DTO.GroupsOfSpecialities;

namespace BLL.DTO.Faculties
{
    public class ArchiveFacultyDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        public List<ArchiveGroupOfSpecialitiesDTO> GroupsOfSpecialities { get; set; } = new();
    }
}
