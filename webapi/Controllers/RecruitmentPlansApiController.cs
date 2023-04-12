using webapi.Data.Models;
using webapi.Data.Repositories;
using webapi.Data.Specifications;
using webapi.ViewModels.RecruitmentPlans;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Services;
using webapi.Data.Interfaces.Services;

namespace webapi.Controllers.Api
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
            IEnumerable<Faculty> faculties = await _facultyRepository.GetAllAsync(new FacultiesSpecification().IncludeRecruitmentPlans());
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification());
            int year = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;

            foreach (Faculty faculty in faculties)
            {
                DetailsFacultyRecruitmentPlans plans = new(faculty, GetFacultyPlans(faculty, year), year);

                model.Add(plans);
            }

            return model;
        }

        [HttpGet("{facultyName}/{year}")]
        public async Task<ActionResult<IEnumerable<PlansForSpecialityVM>>> GetFacultyRecruitmentPlans(string facultyName, int year)
        {
            Faculty? faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties().IncludeRecruitmentPlans());
            List<PlansForSpecialityVM> plansForSpecialities = new();
            if (faculty == null)
            {
                return NotFound();
            }

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToList();
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.FormOfEducation.Year == year).ToList();
                    plansForSpecialities.Add(new(speciality));
                }
            }

            return plansForSpecialities;
        }

        [HttpGet("{facultyName}/lastYear")]
        public async Task<ActionResult<object>> GetFacultyLastYearRecruitmentPlans(string facultyName)
        {
            Faculty? faculty = await _facultyRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties().IncludeRecruitmentPlans());
            List<PlansForSpecialityVM> plansForSpecialities = new();
            int year = 0;

            if (faculty == null)
            {
                return NotFound();
            }

            if (faculty.Specialities != null)
            {
                faculty.Specialities = faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToList();
                List<RecruitmentPlan> allPlans = _plansRepository.GetAllAsync(new RecruitmentPlansSpecification()).Result.Where(p => p.Speciality.Faculty.ShortName == facultyName).ToList();
                year = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;
                foreach (Speciality speciality in faculty.Specialities)
                {
                    speciality.RecruitmentPlans = (speciality.RecruitmentPlans ?? new()).Where(p => p.FormOfEducation.Year == year).ToList();
                    plansForSpecialities.Add(new(speciality));
                }
            }

            return new { year, plansForSpecialities };
        }

        [HttpGet("{facultyName}/{groupId}/GroupRecruitmentPlans")]
        public async Task<ActionResult<IEnumerable<RecruitmentPlan>>> GetGroupRecruitmentPlans(string facultyName, int groupId)
        {
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }

            List<RecruitmentPlan> plans;
            if (!group.IsCompleted)
            {
                plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
                group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions());
                IDistributionService distributionService = new DistributionService(plans, group.Admissions);
                plans = distributionService.GetPlansWithEnrolledStudents();
            }
            else
            {
                plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyName).WhereGroup(group));
            }
            return plans.Select(i => new RecruitmentPlan()
            {
                Id = i.Id,
                Count = i.Count,
                EnrolledStudents = (i.EnrolledStudents ?? new()).Select(s => new EnrolledStudent()
                {
                    Student = new()
                    {
                        Surname = s.Student.Surname,
                        Name = s.Student.Name,
                        Patronymic = s.Student.Patronymic,
                        Id = s.Student.Id,
                        GPS = s.Student.GPS
                    }
                }).ToList(),
                FormOfEducation = i.FormOfEducation,
                PassingScore = i.PassingScore,
                Speciality = new Speciality()
                {
                    Code = i.Speciality.Code,
                    Description = i.Speciality.Description,
                    DirectionCode = i.Speciality.DirectionCode,
                    DirectionName = i.Speciality.DirectionName,
                    FullName = i.Speciality.FullName,
                    Id = i.Speciality.Id,
                    ShortCode = i.Speciality.ShortCode,
                    ShortName = i.Speciality.ShortName,
                    SpecializationCode = i.Speciality.SpecializationCode,
                    SpecializationName = i.Speciality.SpecializationName
                }
            }).ToArray();
        }

        [HttpPut("{facultyName}/{year}")]
        public async Task<IActionResult> PutFacultyRecruitmentPlans(string facultyName, int year, IEnumerable<PlansForSpecialityVM> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereYear(year));
                IEnumerable<RecruitmentPlan> changedPlans = await CreateFacultyPlans(plansForSpecialities, year);
                IEnumerable<RecruitmentPlan> diff = allPlans.Except(changedPlans);

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

        [HttpPost("{facultyName}/{year}")]
        public async Task<ActionResult<RecruitmentPlan>> PostFacultyRecruitmentPlans(string facultyName, int year, IEnumerable<PlansForSpecialityVM> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<RecruitmentPlan> plans = await CreateFacultyPlans(plansForSpecialities, year);
                foreach (RecruitmentPlan plan in plans)
                {
                    await _plansRepository.AddAsync(plan);
                }
                _logger.LogInformation("План приёма на - {FacultyName} - за {Year} создан", facultyName, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{facultyName}/{year}")]
        public async Task<IActionResult> DeleteRecruitmentPlan(string facultyName, int year)
        {
            IEnumerable<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereYear(year));

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

        private async Task<IEnumerable<RecruitmentPlan>> CreateFacultyPlans(IEnumerable<PlansForSpecialityVM> facultyPlans, int year)
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
