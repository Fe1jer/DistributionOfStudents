using ChartJSCore.Models;

namespace BLL.Services.Interfaces
{
    public interface IStatisticService
    {
        Task<Chart> GetPlansStatisticAsync(string facultyName, Guid groupId);
        Task<Chart> GetGroupStatisticAsync(Guid groupId);
        Task SaveAsync(string facultyUrl, Guid groupId);
    }
}
