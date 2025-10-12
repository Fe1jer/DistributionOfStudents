using FilesManager;
using webapi.ViewModels.General;

namespace webapi.ViewModels.Faculties
{
    public class DetailsFacultyViewModel : BaseViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = ManagerConstants.CombinePath(ManagerConstants.SUB_DIR, "Faculties", "Default.jpg");
        public string ImgThumb => FilesHelper.GetCroppedThumbUrl(Img, 300, 170);
    }
}
