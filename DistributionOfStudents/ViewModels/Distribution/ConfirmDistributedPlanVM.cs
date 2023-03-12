namespace DistributionOfStudents.ViewModels.Distribution
{
    public class ConfirmDistributedPlanVM
    {
        public int PlanId { get; set; }
        public string SpecialityName { get; set; } = String.Empty;
        public int PassingScore { get; set; }

        public List<ConfirmDistributedStudentVM> DistributedStudents { get; set; } = new();
    }
}
