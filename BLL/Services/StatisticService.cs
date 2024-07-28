using BLL.DTO;
using BLL.DTO.RecruitmentPlans;
using BLL.Extensions;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using ChartJSCore.Models;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.Specifications;

namespace BLL.Services
{
    public class StatisticService : BaseService, IStatisticService
    {
        public StatisticService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Chart> GetGroupStatisticAsync(Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId);
            List<Dataset> Datasets = new();
            List<double?> data = new();

            foreach (DateTime date in GetRangeDates(group.StartDate, group.EnrollmentDate))
            {
                GroupOfSpecialitiesStatistic? groupStatistic = await _unitOfWork.GroupsOfSpecialitiesStatistic.GetByGroupAndDateAsync(groupId, date);
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

            return chart;
        }

        public async Task<Chart> GetPlansStatisticAsync(string facultyUrl, Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyUrl).IncludeSpecialties());
            IEnumerable<RecruitmentPlan> plans;
            List<Dataset> Datasets = new();

            plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereGroup(group).IncludeSpecialty());
            foreach (var plan in plans)
            {
                List<double?> data = new();
                foreach (DateTime date in GetRangeDates(group.StartDate, group.EnrollmentDate))
                {
                    RecruitmentPlanStatistic? planStatistic = await _unitOfWork.RecruitmentPlansStatistic.GetByPlanAndDateAsync(plan.Id, date);
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

            return chart;
        }

        public async Task SaveAsync(string facultyUrl, Guid groupId)
        {
            GroupOfSpecialities group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties().IncludeAdmissions()) ?? new();
            await UpdateGroupStatisticAsync(group);
            await UpdatePlansStatisticAsync(facultyUrl, group);
            _unitOfWork.Commit();
        }

        private async Task UpdateGroupStatisticAsync(GroupOfSpecialities group)
        {
            GroupOfSpecialitiesStatistic groupStatistic = await _unitOfWork.GroupsOfSpecialitiesStatistic.GetByGroupAndDateAsync(group.Id, DateTime.Today)
                ?? new() { GroupOfSpecialties = group };
            groupStatistic.CountOfAdmissions = (group.Admissions ?? new()).Count;

            await _unitOfWork.GroupsOfSpecialitiesStatistic.InsertOrUpdateAsync(groupStatistic);
        }

        private async Task UpdatePlansStatisticAsync(string facultyName, GroupOfSpecialities group)
        {
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group).WithoutTracking());
            List<RecruitmentPlanDTO> planDtos = Mapper.Map<List<RecruitmentPlanDTO>>(plans);
            List<AdmissionDTO> admissionDtos = Mapper.Map<List<AdmissionDTO>>(group.Admissions);

            foreach (var plan in planDtos.Distribution(admissionDtos).GetPlansWithPassingScores().Keys)
            {
                var plansToStatistic = await _unitOfWork.RecruitmentPlans.GetByIdAsync(plan.Id, new RecruitmentPlansSpecification(i => i.EnrolledStudents == null || i.EnrolledStudents.Count == 0).WithoutTracking()) ?? new();
                RecruitmentPlanStatistic planStatistic = await _unitOfWork.RecruitmentPlansStatistic.GetByPlanAndDateAsync(plan.Id, DateTime.Today)
                    ?? new() { RecruitmentPlan = plansToStatistic };
                planStatistic.Score = plan.PassingScore;

                await _unitOfWork.RecruitmentPlansStatistic.InsertOrUpdateAsync(planStatistic);
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
