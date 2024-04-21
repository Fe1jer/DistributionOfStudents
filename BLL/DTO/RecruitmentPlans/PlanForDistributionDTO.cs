using BLL.DTO.Base;
using BLL.DTO.Students;

namespace BLL.DTO.RecruitmentPlans
{
    public class PlanForDistributionDTO : EntityDTO
    {
        public int PassingScore { get; set; }
        public int Count { get; set; }
        public List<IsDistributedStudentDTO> DistributedStudents { get; set; } = new();
    }
}
