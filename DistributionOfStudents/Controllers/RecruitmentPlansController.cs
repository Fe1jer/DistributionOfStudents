using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/[controller]/[action]")]
    public class RecruitmentPlansController : Controller
    {
        private readonly ILogger<RecruitmentPlansController> _logger;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IFacultiesRepository _facultyRepository;
        private readonly ISpecialitiesRepository _specialityRepository;

        public RecruitmentPlansController(ILogger<RecruitmentPlansController> logger, IFacultiesRepository facultyRepository, IRecruitmentPlansRepository plansRepository, ISpecialitiesRepository specialityRepository)
        {
            _logger = logger;
            _plansRepository = plansRepository;
            _facultyRepository = facultyRepository;
            _specialityRepository = specialityRepository;
        }

        [Route("~/[controller]")]
        public async Task<IActionResult> Index()
        {
            List<DetailsFacultyRecruitmentPlans> model = new();
            List<Faculty> faculties = await _facultyRepository.GetAllAsync(new FacultiesSpecification().IncludeRecruitmentPlans());
            int year = faculties.Count != 0 ? faculties.Select(f => f.Specialities.Count != 0 ? f.Specialities.Select(s => s.RecruitmentPlans.Count != 0 ? s.RecruitmentPlans.Max(p => p.Year) : 0).Max() : 0).Max() : 0;

            foreach (Faculty faculty in faculties)
            {
                DetailsFacultyRecruitmentPlans plans = new()
                {
                    FacultyFullName = faculty.FullName,
                    PlansForSpecialities = GetFacultyPlans(faculty, year),
                    FacultyShortName = faculty.ShortName,
                    Year = year
                };

                model.Add(plans);
            }

            return View(model);
        }

        private static List<PlansForSpecialityVM> GetFacultyPlans(Faculty faculty, int year)
        {
            List<PlansForSpecialityVM> facultyPlans = new();

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => int.Parse(string.Join("", sp.Code.Where(c => char.IsDigit(c))))).ToList();
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = speciality.RecruitmentPlans.Where(p => p.Year == year).ToList();

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                    PlansForSpecialityVM plans = new()
                    {
                        SpecialityName = speciality.FullName,
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

                    facultyPlans.Add(plans);
                }
            }

            return facultyPlans;
        }

        private async Task<List<RecruitmentPlan>> CreateFacultyPlans(List<PlansForSpecialityVM> facultyPlans, int year)
        {
            List<RecruitmentPlan> recruitmentPlans = new();

            if (facultyPlans != null)
            {
                RecruitmentPlan plan;
                foreach (PlansForSpecialityVM plans in facultyPlans)
                {
                    Task<Speciality> getSpeciality = _specialityRepository.GetByIdAsync(plans.SpecialityId);

                    if (plans.DailyFullBudget > 0)
                    {
                        plan = await CreatePlan(plans.DailyFullBudget, true, true, true, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.DailyFullPaid > 0)
                    {
                        plan = await CreatePlan(plans.DailyFullPaid, true, true, false, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.DailyAbbreviatedBudget > 0)
                    {
                        plan = await CreatePlan(plans.DailyAbbreviatedBudget, true, false, true, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.DailyAbbreviatedPaid > 0)
                    {
                        plan = await CreatePlan(plans.DailyAbbreviatedPaid, true, false, false, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.EveningFullBudget > 0)
                    {
                        plan = await CreatePlan(plans.EveningFullBudget, false, true, true, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.EveningFullPaid > 0)
                    {
                        plan = await CreatePlan(plans.EveningFullPaid, false, true, false, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.EveningAbbreviatedBudget > 0)
                    {
                        plan = await CreatePlan(plans.EveningAbbreviatedBudget, false, false, true, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                    if (plans.EveningAbbreviatedPaid > 0)
                    {
                        plan = await CreatePlan(plans.EveningAbbreviatedPaid, false, false, false, year, await getSpeciality);
                        recruitmentPlans.Add(plan);
                    }
                }
            }

            return recruitmentPlans;
        }

        public async Task<RecruitmentPlan> CreatePlan(int count, bool isDailyForm, bool isFullTime, bool isBudget, int year, Speciality speciality)
        {
            RecruitmentPlan plan = (await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereForm(isDailyForm, isFullTime, isBudget, year, speciality.Id))).SingleOrDefault() ?? new();

            plan.Count = count;
            plan.IsDailyForm = isDailyForm;
            plan.IsFullTime = isFullTime;
            plan.IsBudget = isBudget;
            plan.Year = year;
            plan.Speciality = speciality;

            return plan;
        }

        // GET: RecruitmentPlans/Create
        public async Task<IActionResult> Create(string facultyName)
        {
            Faculty faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeRecruitmentPlans());
            if (faculty == null)
            {
                return NotFound();
            }
            int year = faculty.Specialities.Count != 0 ? faculty.Specialities.Select(s => s.RecruitmentPlans.Count != 0 ? s.RecruitmentPlans.Max(p => p.Year) + 1 : DateTime.Now.Year).Max() : DateTime.Now.Year;
            DetailsFacultyRecruitmentPlans model = new()
            {
                PlansForSpecialities = GetFacultyPlans(faculty, year),
                FacultyFullName = faculty.FullName,
                FacultyShortName = faculty.ShortName,
                Year = year,
            };

            return View(model);
        }

        // POST: RecruitmentPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string facultyName, DetailsFacultyRecruitmentPlans model)
        {
            if (ModelState.IsValid)
            {
                List<RecruitmentPlan> plans = await CreateFacultyPlans(model.PlansForSpecialities, model.Year);
                foreach (RecruitmentPlan plan in plans)
                {
                    await _plansRepository.AddAsync(plan);
                }

                return RedirectToAction("Details", "Faculties", new { name = facultyName });
            }
            return View(model);
        }

        // GET: RecruitmentPlans/Edit/5
        public async Task<IActionResult> Edit(string facultyName, int year)
        {
            Faculty faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeRecruitmentPlans());
            if (faculty == null)
            {
                return NotFound();
            }
            DetailsFacultyRecruitmentPlans model = new()
            {
                PlansForSpecialities = GetFacultyPlans(faculty, year),
                FacultyFullName = faculty.FullName,
                FacultyShortName = faculty.ShortName,
                Year = year
            };

            return View(model);
        }

        // POST: RecruitmentPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string facultyName, DetailsFacultyRecruitmentPlans model)
        {
            if (ModelState.IsValid)
            {
                List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereYear(model.Year));
                List<RecruitmentPlan> changedPlans = await CreateFacultyPlans(model.PlansForSpecialities, model.Year);
                List<RecruitmentPlan> diff = allPlans.Except(changedPlans).ToList();

                foreach (RecruitmentPlan plan in diff)
                {
                    await _plansRepository.DeleteAsync(plan.Id);
                }
                foreach (RecruitmentPlan plan in changedPlans)
                {
                    if (plan.Id > 0)
                    {
                        await _plansRepository.UpdateAsync(plan);
                    }
                    else
                    {
                        await _plansRepository.AddAsync(plan);
                    }
                }

                return RedirectToAction("Details", "Faculties", new { name = facultyName });
            }
            return View(model);
        }

        // GET: RecruitmentPlans/Delete/5
        public async Task<IActionResult> Delete(string facultyName, int year)
        {
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereYear(year));

            foreach (RecruitmentPlan plan in allPlans)
            {
                await _plansRepository.DeleteAsync(plan.Id);
            }
            _logger.LogInformation("План приёма на - {FacultyName} -  за {Year} год был удалён", facultyName, year);

            return RedirectToAction("Details", "Faculties", new { name = facultyName });
        }
    }
}
