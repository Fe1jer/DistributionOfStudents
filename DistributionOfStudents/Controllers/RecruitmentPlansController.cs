using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.RecruitmentPlans;
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
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync();
            int year = allPlans.Count != 0 ? allPlans.Max(i => i.Year) : 0;

            foreach (Faculty faculty in faculties)
            {
                DetailsFacultyRecruitmentPlans plans = new(faculty, GetFacultyPlans(faculty, year), year);

                model.Add(plans);
            }

            return View(model);
        }

        // GET: RecruitmentPlans/Create
        public async Task<IActionResult> Create(string facultyName)
        {
            Faculty? faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeRecruitmentPlans());
            if (faculty == null)
            {
                return NotFound();
            }

            List<RecruitmentPlan> allPlans = _plansRepository.GetAllAsync().Result.Where(p => p.Speciality.Faculty.ShortName == facultyName).ToList();
            int year = allPlans.Count != 0 ? allPlans.Max(i => i.Year) + 1 : DateTime.Now.Year;
            DetailsFacultyRecruitmentPlans model = new(faculty, GetFacultyPlans(faculty, year), year);

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
            Faculty? faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeRecruitmentPlans());
            if (faculty == null)
            {
                return NotFound();
            }
            DetailsFacultyRecruitmentPlans model = new(faculty, GetFacultyPlans(faculty, year), year);

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
                    Task task = plan.Id > 0 ? _plansRepository.UpdateAsync(plan) : _plansRepository.AddAsync(plan);
                    await task;
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

        private static List<PlansForSpecialityVM> GetFacultyPlans(Faculty faculty, int year)
        {
            List<PlansForSpecialityVM> facultyPlans = new();

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToList();
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.Year == year).ToList();
                    facultyPlans.Add(new(speciality));
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
                    Speciality? speciality = await _specialityRepository.GetByIdAsync(plans.SpecialityId);
                    if (speciality != null)
                    {
                        if (plans.DailyFullBudget > 0)
                        {
                            plan = await CreatePlan(plans.DailyFullBudget, true, true, true, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.DailyFullPaid > 0)
                        {
                            plan = await CreatePlan(plans.DailyFullPaid, true, true, false, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.DailyAbbreviatedBudget > 0)
                        {
                            plan = await CreatePlan(plans.DailyAbbreviatedBudget, true, false, true, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.DailyAbbreviatedPaid > 0)
                        {
                            plan = await CreatePlan(plans.DailyAbbreviatedPaid, true, false, false, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.EveningFullBudget > 0)
                        {
                            plan = await CreatePlan(plans.EveningFullBudget, false, true, true, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.EveningFullPaid > 0)
                        {
                            plan = await CreatePlan(plans.EveningFullPaid, false, true, false, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.EveningAbbreviatedBudget > 0)
                        {
                            plan = await CreatePlan(plans.EveningAbbreviatedBudget, false, false, true, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
                        if (plans.EveningAbbreviatedPaid > 0)
                        {
                            plan = await CreatePlan(plans.EveningAbbreviatedPaid, false, false, false, year, speciality);
                            recruitmentPlans.Add(plan);
                        }
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
    }
}
