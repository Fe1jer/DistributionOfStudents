using webapi.ViewModels.General;

namespace webapi.ViewModels.Faculties
{
    public class DetailsFacultyViewModel : BaseViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = "\\img\\Faculties\\Default.jpg";
    }
}
