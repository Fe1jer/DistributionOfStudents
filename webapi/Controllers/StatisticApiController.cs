using ChartJSCore.Models;
using webapi.Data.Models;
using webapi.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces.Repositories;

namespace webapi.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticApiController : ControllerBase
    {
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IGroupsOfSpecialitiesStatisticRepository _groupsStatisticRepository;
        private readonly IRecruitmentPlansStatisticRepository _plansStatisticRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;

        public StatisticApiController(IRecruitmentPlansStatisticRepository plansStatisticRepository, IRecruitmentPlansRepository plansRepository,
            IGroupsOfSpecialitiesStatisticRepository groupsStatisticRepository, IGroupsOfSpecialitiesRepository groupsRepository)
        {
            _plansStatisticRepository = plansStatisticRepository;
            _groupsStatisticRepository = groupsStatisticRepository;
            _groupsRepository = groupsRepository;
            _plansRepository = plansRepository;
        }

        [HttpGet("PlansStatisticChart")]
        public async Task<ActionResult<string>> GetPlansStatistic(int groupId, string facultyName)
        {
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeSpecialties());
            List<RecruitmentPlan> plans;
            List<Dataset> Datasets = new();

            if (group == null)
            {
                return NotFound();
            }
            plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group).IncludeSpecialty());
            foreach (var plan in plans)
            {
                List<double?> data = new();
                foreach (DateTime date in GetRangeDates(group.StartDate, group.EnrollmentDate))
                {
                    RecruitmentPlanStatistic? planStatistic = await _plansStatisticRepository.GetByPlanAndDateAsync(plan.Id, date);
                    data.Add((planStatistic ?? new RecruitmentPlanStatistic()).Score);
                }
                LineDataset dataset = new()
                {
                    Label = plan.Speciality.DirectionName ?? plan.Speciality.FullName,
                    Data = data
                };
                Datasets.Add(dataset);
            }
            Chart chart = new()
            {
                Data = new()
                {
                    Labels = GetRangeDates(group.StartDate, group.EnrollmentDate).Select(i => i.ToString("dd.MM")).ToArray(),
                    Datasets = Datasets
                }
            };

            return new ActionResult<string>(chart.SerializeBody());
        }

        [HttpGet("GroupStatisticChart")]
        public async Task<ActionResult<string>> GetGroupStatistic(int groupId)
        {
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId);
            List<Dataset> Datasets = new();

            if (group == null)
            {
                return NotFound();
            }
            List<double?> data = new();
            foreach (DateTime date in GetRangeDates(group.StartDate, group.EnrollmentDate))
            {
                GroupOfSpecialitiesStatistic? groupStatistic = await _groupsStatisticRepository.GetByGroupAndDateAsync(groupId, date);
                data.Add((groupStatistic ?? new GroupOfSpecialitiesStatistic()).CountOfAdmissions);
            }
            LineDataset dataset = new()
            {
                Label = group.Name,
                Data = data
            };
            Datasets.Add(dataset);

            Chart chart = new()
            {
                Data = new()
                {
                    Labels = GetRangeDates(group.StartDate, group.EnrollmentDate).Select(i => i.ToString("dd.MM")).ToArray(),
                    Datasets = Datasets
                }
            };

            return new ActionResult<string>(chart.SerializeBody());
        }

        private static List<DateTime> GetRangeDates(DateTime start, DateTime finish)
        {
            var dates = new List<DateTime>();
            for (var dt = start; dt <= (DateTime.Today < finish ? DateTime.Today : finish); dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            return dates;
        }
    }
}
