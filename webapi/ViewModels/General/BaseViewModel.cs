namespace webapi.ViewModels.General
{
    public abstract class BaseViewModel
    {
        public Guid Id { get; set; }
        public bool IsNew => Id == Guid.Empty;
    }
}
