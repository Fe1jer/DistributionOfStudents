using webapi.ViewModels.GroupsOfSpecialities;

namespace webapi.ViewModels.Archive
{
    public class ArchiveFacultyViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public List<ArchiveGroupOfSpecialitiesViewModel> GroupsOfSpecialities { get; set; } = new();
    }
}
