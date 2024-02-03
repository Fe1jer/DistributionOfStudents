using webapi.ViewModels.GroupsOfSpecialities;

namespace webapi.ViewModels.Archive
{
    public class DetailsFacultyArchive
    {
        public string FacultyFullName { get; set; } = string.Empty;
        public List<DetailsGroupOfSpecialitiesVM> GroupsOfSpecialities { get; set; } = new();
    }
}
