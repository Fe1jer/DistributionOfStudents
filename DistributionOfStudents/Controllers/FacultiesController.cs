using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Services;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Faculties;
using DistributionOfStudents.ViewModels.GroupsOfSpecialities;
using DistributionOfStudents.ViewModels.RecruitmentPlans;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly ILogger<FacultiesController> _logger;
        private readonly IFacultiesRepository _facultyRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;

        public FacultiesController(ILogger<FacultiesController> logger, IFacultiesRepository facultyRepository, IGroupsOfSpecialitiesRepository groupRepository, IRecruitmentPlansRepository planRepository)
        {
            _logger = logger;
            _facultyRepository = facultyRepository;
            _groupsRepository = groupRepository;
            _plansRepository = planRepository;
        }

        // GET: FacultiesController
        public IActionResult Index() => View();

        // GET: FacultiesController/Details/5
        [Route("[controller]/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            Faculty? faculty = await _facultyRepository.GetByShortNameAsync(name, new FacultiesSpecification().IncludeSpecialties().IncludeRecruitmentPlans());
            List<GroupOfSpecialties> groups = new();
            List<DetailsGroupOfSpecialitiesVM> groupsOfSpecialities = new();
            DetailsFacultyRecruitmentPlans FacultyPlans = new();
            if (faculty == null)
            {
                return NotFound();
            }

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToList();
                List<RecruitmentPlan> allPlans = _plansRepository.GetAllAsync(new RecruitmentPlansSpecification()).Result.Where(p => p.Speciality.Faculty.ShortName == name).ToList();
                FacultyPlans.Year = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;
                FacultyPlans.FacultyFullName = faculty.FullName;
                FacultyPlans.FacultyShortName = faculty.ShortName;
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.FormOfEducation.Year == FacultyPlans.Year).ToList();
                    FacultyPlans.PlansForSpecialities.Add(new(speciality));
                }
            }

            groups = await _groupsRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(faculty.ShortName).IncludeAdmissions().IncludeSpecialties().WhereYear(FacultyPlans.Year));
            foreach (GroupOfSpecialties group in groups)
            {
                List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(faculty.ShortName).WhereGroup(group));
                DistributionService distributionService = new(plans, group.Admissions);
                groupsOfSpecialities.Add(new DetailsGroupOfSpecialitiesVM(group, plans, distributionService.Competition));
            }

            DetailsFacultyVM model = new(faculty, FacultyPlans, groupsOfSpecialities);

            return View(model);
        }

        // GET: FacultiesController/Create
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("[controller]/[action]")]
        public IActionResult Edit(string shortName)
        {
            return View();
        }
    }
}
