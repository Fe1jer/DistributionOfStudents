using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistributionOfStudents.Data;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels;
using DistributionOfStudents.Data.Services;
using System.Numerics;

namespace DistributionOfStudents.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly ILogger<FacultiesController> _logger;
        private readonly IFacultiesRepository _facultyRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupRepository;
        private readonly IRecruitmentPlansRepository _planRepository;

        public FacultiesController(ILogger<FacultiesController> logger, IFacultiesRepository facultyRepository, IGroupsOfSpecialitiesRepository groupRepository, IRecruitmentPlansRepository planRepository)
        {
            _logger = logger;
            _facultyRepository = facultyRepository;
            _groupRepository = groupRepository;
            _planRepository = planRepository;
        }

        // GET: FacultiesController
        public async Task<IActionResult> Index() => View(await _facultyRepository.GetAllAsync());

        // GET: FacultiesController/Details/5
        [Route("[controller]/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            Faculty faculty = (await _facultyRepository.GetAllAsync(new FacultiesSpecification().IncludeSpecialties().IncludeRecruitmentPlans().WhereShortName(name))).Single();
            List<GroupOfSpecialties> groups = new();
            List<DetailsGroupOfSpecialitiesVM> groupsOfSpecialities = new();
            DetailsFacultyRecruitmentPlans FacultyPlans = new()
            {
                PlansForSpecialities = new List<PlansForSpecialityVM>()
            };

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => int.Parse(string.Join("", sp.Code.Where(c => char.IsDigit(c))))).ToList();
                FacultyPlans.Year = faculty.Specialities.Count != 0 ? faculty.Specialities.Select(s => s.RecruitmentPlans.Count != 0 ? s.RecruitmentPlans.Max(p => p.Year) : 0).Max() : 0;
                FacultyPlans.FacultyFullName = faculty.FullName;
                FacultyPlans.FacultyShortName = faculty.ShortName;
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = speciality.RecruitmentPlans.Where(p => p.Year == FacultyPlans.Year).ToList();

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                    PlansForSpecialityVM plans = new()
                    {
                        SpecialityName = speciality.DirectionName ?? speciality.FullName,
                        SpecialityId = speciality.Id,
                        DailyFullBudget = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && p.IsFullTime && p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && p.IsFullTime && p.IsBudget).Count : 0,
                        DailyFullPaid = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && p.IsFullTime && !p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && p.IsFullTime && !p.IsBudget).Count : 0,
                        DailyAbbreviatedBudget = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && !p.IsFullTime && p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && !p.IsFullTime && p.IsBudget).Count : 0,
                        DailyAbbreviatedPaid = speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && !p.IsFullTime && !p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => p.IsDailyForm && !p.IsFullTime && !p.IsBudget).Count : 0,
                        EveningFullBudget = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && p.IsFullTime && p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && p.IsFullTime && p.IsBudget).Count : 0,
                        EveningFullPaid = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && p.IsFullTime && !p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && p.IsFullTime && !p.IsBudget).Count : 0,
                        EveningAbbreviatedBudget = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && !p.IsFullTime && p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && !p.IsFullTime && p.IsBudget).Count : 0,
                        EveningAbbreviatedPaid = speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && !p.IsFullTime && !p.IsBudget) != null
                        ? speciality.RecruitmentPlans.FirstOrDefault(p => !p.IsDailyForm && !p.IsFullTime && !p.IsBudget).Count : 0
                    };
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

                    FacultyPlans.PlansForSpecialities.Add(plans);
                }
            }

            groups = await _groupRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(faculty.ShortName).IncludeSpecialties().WhereYear(FacultyPlans.Year));
            foreach (GroupOfSpecialties group in groups)
            {
                List<RecruitmentPlan> plans = await _planRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(faculty.ShortName).WhereGroup(group));
                plans = plans.Where(p => group.Specialities.Contains(p.Speciality)).ToList();
                groupsOfSpecialities.Add(new DetailsGroupOfSpecialitiesVM() { GroupOfSpecialties = group, RecruitmentPlans = plans, Year = FacultyPlans.Year, FacultyShortName = faculty.ShortName });
            }

            DetailsFacultyVM model = new()
            {
                Faculty = faculty,
                GroupsOfSpecialties = groupsOfSpecialities,
                AllPlansForSpecialities = FacultyPlans
            };

            return View(model);
        }

        // GET: FacultiesController/Create
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            CreateChangeFacultyVM model = new()
            {
                Faculty = new Faculty() { Img = "\\img\\Faculties\\Default.jpg" }
            };
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
                    Faculty? sameFaculty = (await _facultyRepository.GetAllAsync(new FacultiesSpecification().WhereShortName(model.Faculty.ShortName))).SingleOrDefault();
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
            Faculty faculty = await _facultyRepository.GetByIdAsync(id, new FacultiesSpecification());

            CreateChangeFacultyVM model = new()
            {
                Faculty = faculty,
            };

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
                        FileService.DeleteFile(model.Faculty.Img);
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
            Faculty faculty = await _facultyRepository.GetByIdAsync(id);
            await _facultyRepository.DeleteAsync(id);
            _logger.LogInformation("Факультет - {FacultyName} - был удалён", faculty.FullName);

            return RedirectToAction(nameof(Index));
        }
    }
}
