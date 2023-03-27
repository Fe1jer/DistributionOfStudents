namespace webapi.ViewModels.Distribution
{
    public class CreateDistributionVM
    {
        public string GroupName { get; set; } = String.Empty;

        public List<PlanForDistributionVM> Plans { get; set; } = new();
    }
}
