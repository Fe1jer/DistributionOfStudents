namespace webapi.ViewModels.Distribution
{
    public class ConfirmDistributionVM
    {
        public string GroupName { get; set; } = String.Empty;

        public List<ConfirmDistributedPlanVM> Plans { get; set; } = new();
    }
}
