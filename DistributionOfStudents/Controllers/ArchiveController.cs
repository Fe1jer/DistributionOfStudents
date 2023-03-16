using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Services;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Archive;
using DistributionOfStudents.ViewModels.GroupsOfSpecialities;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IFacultiesRepository _facultiesRepository;

        public ArchiveController(IRecruitmentPlansRepository plansRepository, IGroupsOfSpecialitiesRepository groupsRepository, IFacultiesRepository facultiesRepository)
        {
            _plansRepository = plansRepository;
            _groupsRepository = groupsRepository;
            _facultiesRepository = facultiesRepository;
        }

        // GET: ArchiveController
        public async Task<IActionResult> Index()
        {
            int maxYear = DateTime.Now.Year - 1;
            List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.FormOfEducation.Year <= maxYear));
            List<int> allYears = groups.Count != 0 ? groups.Select(i => i.FormOfEducation.Year).Distinct().OrderBy(i => i).ToList() : new();

            return View(allYears);
        }

        [Route("[controller]/{year}")]
        public async Task<IActionResult> SelectFormOfEducation(int year)
        {
            List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification().WhereYear(year));
            List<string> allforms = groups.Select(i => i.Name).Distinct().ToList();

            return View(allforms);
        }

        [Route("[controller]/{year}/{form}")]
        public async Task<IActionResult> Details(int year, string form)
        {
            List<DetailsFacultyArchive> models = new();
            DistributionService distributionService;

            foreach (Faculty faculty in await _facultiesRepository.GetAllAsync())
            {
                List<DetailsGroupOfSpecialitiesVM> groupsOfSpecialities = new();
                List<GroupOfSpecialties> groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.Name == form)
                    .WhereFaculty(faculty.ShortName).WhereCompleted().IncludeAdmissions().IncludeSpecialties().WhereYear(year));

                foreach (GroupOfSpecialties group in groups.OrderBy(i => (i.Specialities ?? new()).First().Code))
                {
                    List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(faculty.ShortName).WhereGroup(group));
                    distributionService = new(plans, group.Admissions);
                    groupsOfSpecialities.Add(new DetailsGroupOfSpecialitiesVM(group, plans, distributionService.Competition));
                }
                if (groupsOfSpecialities.Count != 0)
                {
                    models.Add(new(faculty.FullName, groupsOfSpecialities));
                }
            }

            return View(models);
        }
    }
}
