namespace webapi.ViewModels
{
    public abstract class BaseViewModel
    {
        public Guid Id { get; set; }
        public bool IsNew => Id == Guid.Empty;
    }
}
