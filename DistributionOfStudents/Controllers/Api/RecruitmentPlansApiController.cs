using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.RecruitmentPlans;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentPlansApiController : ControllerBase
    {
        private readonly ILogger<RecruitmentPlansApiController> _logger;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IFacultiesRepository _facultyRepository;
        private readonly ISpecialitiesRepository _specialityRepository;
        private readonly IFormsOfEducationRepository _formsOfEducationRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;

        public RecruitmentPlansApiController(ILogger<RecruitmentPlansApiController> logger, IFacultiesRepository facultyRepository, IRecruitmentPlansRepository plansRepository,
            ISpecialitiesRepository specialityRepository, IFormsOfEducationRepository formsOfEducationRepository, IGroupsOfSpecialitiesRepository groupsRepository)
        {
            _logger = logger;
            _plansRepository = plansRepository;
            _facultyRepository = facultyRepository;
            _specialityRepository = specialityRepository;
            _formsOfEducationRepository = formsOfEducationRepository;
            _groupsRepository = groupsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailsFacultyRecruitmentPlans>>> GetFacultiesRecruitmentPlans()
        {
            List<DetailsFacultyRecruitmentPlans> model = new();
            List<Faculty> faculties = await _facultyRepository.GetAllAsync(new FacultiesSpecification().IncludeRecruitmentPlans());
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification());
            int year = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;

            foreach (Faculty faculty in faculties)
            {
                DetailsFacultyRecruitmentPlans plans = new(faculty, GetFacultyPlans(faculty, year), year);

                model.Add(plans);
            }

            return model;
        }

        [HttpGet("{facultyName}")]
        public async Task<ActionResult<DetailsFacultyRecruitmentPlans>> GetFacultyRecruitmentPlans(string facultyName, int? year)
        {
            Faculty? faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties().IncludeRecruitmentPlans());
            DetailsFacultyRecruitmentPlans facultyPlans = new();
            if (faculty == null)
            {
                return NotFound();
            }

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToList();
                List<RecruitmentPlan> allPlans = _plansRepository.GetAllAsync(new RecruitmentPlansSpecification()).Result.Where(p => p.Speciality.Faculty.ShortName == facultyName).ToList();
                facultyPlans.Year = year ?? (allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0);
                facultyPlans.FacultyFullName = faculty.FullName;
                facultyPlans.FacultyShortName = faculty.ShortName;
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.FormOfEducation.Year == facultyPlans.Year).ToList();
                    facultyPlans.PlansForSpecialities.Add(new(speciality));
                }
            }

            return facultyPlans;
        }

        [HttpGet("{facultyName}/GroupRecruitmentPlans")]
        public async Task<ActionResult<List<RecruitmentPlan>>> GetGroupRecruitmentPlans(string facultyName, int groupId)
        {
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }

            return new ActionResult<List<RecruitmentPlan>>(await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyName).WhereGroup(group)));
        }

        [HttpPut("{facultyName}")]
        public async Task<IActionResult> PutFacultyRecruitmentPlans(string facultyName, int year, List<PlansForSpecialityVM> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereYear(year));
                List<RecruitmentPlan> changedPlans = await CreateFacultyPlans(plansForSpecialities, year);
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
                _logger.LogInformation("План приёма на - {FacultyName} - за {Year} обновлён", facultyName, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{facultyName}")]
        public async Task<ActionResult<RecruitmentPlan>> PostFacultyRecruitmentPlans(string facultyName, int year, List<PlansForSpecialityVM> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                List<RecruitmentPlan> plans = await CreateFacultyPlans(plansForSpecialities, year);
                foreach (RecruitmentPlan plan in plans)
                {
                    await _plansRepository.AddAsync(plan);
                }
                _logger.LogInformation("План приёма на - {FacultyName} - за {Year} создан", facultyName, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{facultyName}")]
        public async Task<IActionResult> DeleteRecruitmentPlan(string facultyName, int year)
        {
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereYear(year));

            foreach (RecruitmentPlan plan in allPlans)
            {
                await _plansRepository.DeleteAsync(plan.Id);
            }
            _logger.LogInformation("План приёма на - {FacultyName} - за {Year} год удалён", facultyName, year);

            return Ok();
        }

        private static List<PlansForSpecialityVM> GetFacultyPlans(Faculty faculty, int year)
        {
            List<PlansForSpecialityVM> facultyPlans = new();

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToList();
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.FormOfEducation.Year == year).ToList();
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
            FormOfEducation form = new()
            {
                IsDailyForm = isDailyForm,
                IsBudget = isBudget,
                IsFullTime = isFullTime,
                Year = year,
            };
            RecruitmentPlan plan = (await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereForm(form).WhereSpeciality(speciality.Id))).SingleOrDefault() ?? new();
            form = await GetOrCreateFormFromDB(form);
            plan.Count = count;
            plan.FormOfEducation = form;
            plan.Speciality = speciality;

            return plan;
        }

        private async Task<FormOfEducation> GetOrCreateFormFromDB(FormOfEducation form)
        {
            FormOfEducation formOfEducation = _formsOfEducationRepository.GetAllAsync(new FormOfEducationSpecification().WhereForm(form)).Result.SingleOrDefault() ?? form;

            if (formOfEducation.Id == 0)
            {
                await _formsOfEducationRepository.AddAsync(form);
            }

            return _formsOfEducationRepository.GetAllAsync(new FormOfEducationSpecification().WhereForm(form)).Result.Single();
        }
    }
}
