using BLL.DTO.Base;

namespace BLL.DTO.Subjects
{
    public class SelectedSubjectDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
