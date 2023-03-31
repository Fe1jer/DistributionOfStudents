using ChartJSCore.Models;
using webapi.Data.Models;
using webapi.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Services;

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
            IEnumerable<RecruitmentPlan> plans;
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

        [HttpPut("{facultyName}/{groupId}")]
        public async Task<ActionResult> PostGroupStatistic(string facultyName, int groupId)
        {
            GroupOfSpecialties group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties().IncludeAdmissions()) ?? new();
            await UpdateGroupStatisticAsync(group);
            await UpdatePlansStatisticAsync(facultyName, group);

            return Ok();
        }

        private async Task UpdateGroupStatisticAsync(GroupOfSpecialties group)
        {
            GroupOfSpecialitiesStatistic groupStatistic = await _groupsStatisticRepository.GetByGroupAndDateAsync(group.Id, DateTime.Today)
                ?? new() { Date = DateTime.Today, GroupOfSpecialties = group };
            groupStatistic.CountOfAdmissions = (group.Admissions ?? new()).Count;
            Task task = groupStatistic.Id != 0 ? _groupsStatisticRepository.UpdateAsync(groupStatistic) : _groupsStatisticRepository.AddAsync(groupStatistic);
            await task;
        }

        private async Task UpdatePlansStatisticAsync(string facultyName, GroupOfSpecialties group)
        {
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            DistributionService distributionService = new(plans, group.Admissions);
            plans = distributionService.GetPlansWithEnrolledStudents();
            foreach (var plan in plans)
            {
                RecruitmentPlanStatistic planStatistic = await _plansStatisticRepository.GetByPlanAndDateAsync(plan.Id, DateTime.Today)
                    ?? new() { Date = DateTime.Today, RecruitmentPlan = await _plansRepository.GetByIdAsync(plan.Id) ?? new() };
                planStatistic.Score = plan.PassingScore;
                Task task = planStatistic.Id != 0 ? _plansStatisticRepository.UpdateAsync(planStatistic) : _plansStatisticRepository.AddAsync(planStatistic);
                await task;
            }
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
