using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Services;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Archive;
using DistributionOfStudents.ViewModels.GroupsOfSpecialities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveApiController : ControllerBase
    {
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IFacultiesRepository _facultiesRepository;

        public ArchiveApiController(IRecruitmentPlansRepository plansRepository, IGroupsOfSpecialitiesRepository groupsRepository, IFacultiesRepository facultiesRepository)
        {
            _plansRepository = plansRepository;
            _groupsRepository = groupsRepository;
            _facultiesRepository = facultiesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetArchive()
        {
            int maxYear = DateTime.Now.Year - 1;
            List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.FormOfEducation.Year <= maxYear));
            List<int> allYears = groups.Count != 0 ? groups.Select(i => i.FormOfEducation.Year).Distinct().OrderBy(i => i).ToList() : new();

            return allYears;
        }

        [HttpGet("GetArchveFormsByYear/{year}")]
        public async Task<ActionResult<IEnumerable<string>>> GetArchveFormsByYear(int year)
        {
            List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification().WhereYear(year));
            List<string> allforms = groups.Select(i => i.Name).Distinct().ToList();

            return allforms;
        }

        [HttpGet("GetArchveByYearAndForm/{year}/{form}")]
        public async Task<ActionResult<IEnumerable<DetailsFacultyArchive>>> GetArchveByYearAndForm(int year, string form)
        {
            List<DetailsFacultyArchive> models = new();
            DistributionService distributionService;

            foreach (Faculty faculty in await _facultiesRepository.GetAllAsync())
            {
                List<DetailsGroupOfSpecialitiesVM> groupsOfSpecialities = new();
                List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.Name == form)
                    .WhereFaculty(faculty.ShortName).WhereYear(year).WhereCompleted().IncludeAdmissions().IncludeSpecialties());

                foreach (GroupOfSpecialties group in groups.OrderBy(i => (i.Specialities ?? new()).First().Code))
                {
                    List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(faculty.ShortName).WhereGroup(group));
                    distributionService = new(plans, group.Admissions);
                    var a = new DetailsGroupOfSpecialitiesVM(group.Name, plans
                        .Select(i => new RecruitmentPlan()
                        {
                            PassingScore = i.PassingScore,
                            Speciality = new Speciality() { Code = i.Speciality.Code, DirectionCode = i.Speciality.DirectionCode, FullName = i.Speciality.FullName, DirectionName = i.Speciality.DirectionName }

                        }).ToList(), distributionService.Competition);
                    groupsOfSpecialities.Add(a);
                }
                if (groupsOfSpecialities.Count != 0)
                {
                    models.Add(new(faculty.FullName, groupsOfSpecialities));
                }
            }

            return models;
        }
    }
}
