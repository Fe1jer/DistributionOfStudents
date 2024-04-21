using BLL.DTO.Base;

namespace BLL.DTO.Specialities
{
    public class SelectedSpecialityDTO : EntityDTO
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
