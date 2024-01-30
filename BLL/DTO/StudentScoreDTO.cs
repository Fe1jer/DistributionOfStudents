using BLL.DTO.Base;

namespace BLL.DTO
{
    public class StudentScoreDTO : EntityDTO
    {
        public SubjectDTO Subject { get; set; } = new();
        public int Score { get; set; }
    }
}