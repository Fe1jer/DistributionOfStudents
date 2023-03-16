using DistributionOfStudents.ViewModels.GroupsOfSpecialities;

namespace DistributionOfStudents.ViewModels.Archive
{
    public class DetailsFacultyArchive
    {
        public DetailsFacultyArchive() { }
        public DetailsFacultyArchive(string facultyFullName, List<DetailsGroupOfSpecialitiesVM> groups)
        {
            FacultyFullName= facultyFullName;
            GroupsOfSpecialities = groups;
        }

        public string FacultyFullName { get; set; } = string.Empty;
        public List<DetailsGroupOfSpecialitiesVM> GroupsOfSpecialities { get; set; } = new();
    }
}
