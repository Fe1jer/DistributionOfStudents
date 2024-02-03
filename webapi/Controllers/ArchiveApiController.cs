using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Interfaces.Services;
using webapi.Data.Models;
using webapi.Data.Services;
using webapi.Data.Specifications;
using webapi.ViewModels.Archive;
using webapi.ViewModels.GroupsOfSpecialities;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveApiController : BaseController
    {
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IFacultiesRepository _facultiesRepository;

        public ArchiveApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<AdmissionsApiController> logger, IRecruitmentPlansRepository plansRepository, IGroupsOfSpecialitiesRepository groupsRepository, IFacultiesRepository facultiesRepository) : base(accessor, generator)
        {
            _plansRepository = plansRepository;
            _groupsRepository = groupsRepository;
            _facultiesRepository = facultiesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetArchive()
        {
            int maxYear = DateTime.Now.Year - 1;
            List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.FormOfEducation.Year <= maxYear).WhereCompleted());
            List<int> allYears = groups.Count != 0 ? groups.Select(i => i.FormOfEducation.Year).Distinct().OrderBy(i => i).ToList() : new();

            return allYears;
        }

        [HttpGet("GetArchveFormsByYear/{year}")]
        public async Task<ActionResult<IEnumerable<string>>> GetArchveFormsByYear(int year)
        {
            List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification().WhereCompleted().WhereYear(year));
            List<string> allforms = groups.Select(i => i.Name).Distinct().ToList();

            return allforms;
        }

        [HttpGet("GetArchveByYearAndForm/{year}/{form}")]
        public async Task<ActionResult<IEnumerable<DetailsFacultyArchive>>> GetArchveByYearAndForm(int year, string form)
        {
            List<DetailsFacultyArchive> models = new();
            IDistributionService distributionService;

            foreach (Faculty faculty in await _facultiesRepository.GetAllAsync())
            {
                List<DetailsGroupOfSpecialitiesVM> groupsOfSpecialities = new();
                List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.Name == form)
                    .WhereFaculty(faculty.ShortName).WhereYear(year).WhereCompleted().IncludeAdmissions().IncludeSpecialties());

                foreach (GroupOfSpecialties group in groups.OrderBy(i => (i.Specialities ?? new()).First().Code))
                {
                    List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification()
                        .IncludeEnrolledStudents().WhereFaculty(faculty.ShortName).WhereGroup(group));
                    distributionService = new DistributionService(plans, group.Admissions);
                    plans.ForEach(plan => plan.Speciality.Faculty = new());
                    plans.ForEach(plan => plan.Speciality.GroupsOfSpecialties = null);
                    plans.ForEach(plan => plan.Speciality.RecruitmentPlans = null);
                    plans.ForEach(plan => plan.EnrolledStudents = null);
                    var details = new DetailsGroupOfSpecialitiesVM(group.Name, plans, distributionService.Competition);
                    groupsOfSpecialities.Add(details);
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
