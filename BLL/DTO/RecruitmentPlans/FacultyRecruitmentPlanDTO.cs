namespace BLL.DTO.RecruitmentPlans
{
    public class FacultyRecruitmentPlanDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public int Year { get; set; }
        public List<RecruitmentPlanDTO> PlansForSpecialities { get; set; } = new();
    }
}
