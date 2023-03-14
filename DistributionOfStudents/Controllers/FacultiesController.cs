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
        private readonly IGroupsOfSpecialitiesRepository _groupRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;

        public FacultiesController(ILogger<FacultiesController> logger, IFacultiesRepository facultyRepository, IGroupsOfSpecialitiesRepository groupRepository, IRecruitmentPlansRepository planRepository)
        {
            _logger = logger;
            _facultyRepository = facultyRepository;
            _groupRepository = groupRepository;
            _plansRepository = planRepository;
        }

        // GET: FacultiesController
        public async Task<IActionResult> Index() => View(await _facultyRepository.GetAllAsync());

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
                List<RecruitmentPlan> allPlans = _plansRepository.GetAllAsync().Result.Where(p => p.Speciality.Faculty.ShortName == name).ToList();
                FacultyPlans.Year = allPlans.Count != 0 ? allPlans.Max(i => i.Year) : 0;
                FacultyPlans.FacultyFullName = faculty.FullName;
                FacultyPlans.FacultyShortName = faculty.ShortName;
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.Year == FacultyPlans.Year).ToList();
                    FacultyPlans.PlansForSpecialities.Add(new(speciality));
                }
            }

            groups = await _groupRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(faculty.ShortName).IncludeAdmissions().IncludeSpecialties().WhereYear(FacultyPlans.Year));
            foreach (GroupOfSpecialties group in groups)
            {
                List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(faculty.ShortName).WhereGroup(group));
                DistributionService distributionService = new(plans, group.Admissions);
                groupsOfSpecialities.Add(new DetailsGroupOfSpecialitiesVM(group, plans, FacultyPlans.Year, distributionService.Competition));
            }

            DetailsFacultyVM model = new(faculty, FacultyPlans, groupsOfSpecialities);

            return View(model);
        }

        // GET: FacultiesController/Create
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            CreateChangeFacultyVM model = new(new Faculty() { Img = "\\img\\Faculties\\Default.jpg" });
            return View(model);
        }

        // POST: FacultiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create(CreateChangeFacultyVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Faculty? sameFaculty = await _facultyRepository.GetByShortNameAsync(model.Faculty.ShortName, new FacultiesSpecification());
                    if (sameFaculty == null)
                    {
                        string path = "\\img\\Faculties\\";
                        path += model.Img != null ? model.Faculty.ShortName.Replace(" ", "_") + "\\" : "Default.jpg";

                        Faculty faculty = new()
                        {
                            FullName = model.Faculty.FullName,
                            ShortName = model.Faculty.ShortName,
                            Img = model.Img != null ? FileService.UploadFile(model.Img, path + model.Img.FileName) : path
                        };

                        Task addFaculty = _facultyRepository.AddAsync(faculty);
                        if (model.Img != null)
                        {
                            FileService.ResizeAndCrop(faculty.Img, 300, 170);
                        }

                        await addFaculty;
                        _logger.LogInformation("Создан факультет - {FacultyName}", faculty.FullName);

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Такой факультет уже существует");
                        return View(model);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError("Произошла ошибка при создании факультета");

                return View(model);
            }
        }

        // GET: FacultiesController/Edit/5
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Edit(int id)
        {
            Faculty? faculty = await _facultyRepository.GetByIdAsync(id, new FacultiesSpecification());

            if (faculty == null)
            {
                return NotFound();
            }

            CreateChangeFacultyVM model = new(faculty);

            return View(model);
        }

        // POST: FacultiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Edit(CreateChangeFacultyVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string path = "\\img\\Faculties\\";
                    path += model.Img != null ? model.Faculty.ShortName.Replace(" ", "_") + "\\" : "Default.jpg";

                    if (model.Img != null && model.Faculty.Img != "\\img\\Faculties\\Default.jpg")
                    {
                        string[] deletePath = model.Faculty.Img.Split('.');
                        model.Faculty.Img = deletePath[0] + "_300x170." + deletePath[1];
                        FileService.DeleteFile(model.Faculty.Img);
                        model.Faculty.Img = FileService.UploadFile(model.Img, path + model.Img.FileName);
                        FileService.ResizeAndCrop(model.Faculty.Img, 300, 170);
                    }
                    else if (model.Img != null)
                    {
                        model.Faculty.Img = FileService.UploadFile(model.Img, path + model.Img.FileName);
                        FileService.ResizeAndCrop(model.Faculty.Img, 300, 170);
                    }

                    await _facultyRepository.UpdateAsync(model.Faculty);
                    _logger.LogInformation("Изменён факультет - {FacultyName}", model.Faculty.FullName);

                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError("Произошла ошибка при изменении факультета - {FacultyName}", model.Faculty.ShortName);

                return View(model);
            }
        }

        // GET: FacultiesController/Delete/5
        [Route("[controller]/[action]")]
        public async Task<RedirectToActionResult> DeleteAsync(int id)
        {
            Faculty? faculty = await _facultyRepository.GetByIdAsync(id);
            if (faculty != null)
            {
                await _facultyRepository.DeleteAsync(id);
                _logger.LogInformation("Факультет - {FacultyName} - был удалён", faculty.FullName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
