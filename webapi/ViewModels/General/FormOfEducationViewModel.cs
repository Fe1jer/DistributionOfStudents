namespace webapi.ViewModels.General
{
    public class FormOfEducationViewModel : BaseViewModel
    {
        public int Year { get; set; }
        public bool IsDailyForm { get; set; }
        public bool IsBudget { get; set; }
        public bool IsFullTime { get; set; }
    }
}
