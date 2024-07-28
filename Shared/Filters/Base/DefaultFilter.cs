using Microsoft.AspNetCore.Mvc;

namespace Shared.Filters.Base
{
    [BindProperties]
    public class DefaultFilter
    {
        public int Page { get; set; } = 1;
        public int PageLimit { get; set; }
        public int Skip => (Page - 1) * PageLimit;
        public string? Search { get; set; }
    }
}
