namespace Shared.Filters.Base
{
    public class DefaultFilter
    {
        public int Page { get; set; }
        public int PageLimit { get; set; }
        public int Skip => (Page - 1) * PageLimit;
        public string Search { get; set; }
    }
}
