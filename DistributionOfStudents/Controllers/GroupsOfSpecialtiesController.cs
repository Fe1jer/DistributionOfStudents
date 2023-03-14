using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories;
using DistributionOfStudents.Data.Services;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.GroupsOfSpecialities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/[controller]/[action]")]
    public class GroupsOfSpecialtiesController : Controller
    {
        private readonly ILogger<GroupsOfSpecialtiesController> _logger;
        private readonly IFacultiesRepository _facultiesRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsOfSpecialtiesRepository;
        private readonly ISpecialitiesRepository _specialtiesRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IFormsOfEducationRepository _formsOfEducationRepository;

        public GroupsOfSpecialtiesController(ILogger<GroupsOfSpecialtiesController> logger, IFacultiesRepository facultiesRepository,
            IGroupsOfSpecialitiesRepository groupsOfSpecialtiesRepository, ISpecialitiesRepository specialtiesRepository,
            IRecruitmentPlansRepository plansRepository, ISubjectsRepository subjectsRepository, IFormsOfEducationRepository formsOfEducationRepository)
        {
            _logger = logger;
            _facultiesRepository = facultiesRepository;
            _groupsOfSpecialtiesRepository = groupsOfSpecialtiesRepository;
            _specialtiesRepository = specialtiesRepository;
            _plansRepository = plansRepository;
            _subjectsRepository = subjectsRepository;
            _formsOfEducationRepository = formsOfEducationRepository;
        }

        [Route("~/Faculties/{facultyName}/{id}")]
        public async Task<IActionResult> Details(string facultyName, int id, string? searchStudents)
        {
            List<RecruitmentPlan> plans;
            DistributionService distributionService;
            GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }
            if (!group.IsCompleted)
            {
                plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
                distributionService = new(plans, group.Admissions);
                plans = distributionService.GetPlansWithEnrolledStudents();
                group.Admissions = SearchAdmissions(searchStudents, group.Admissions).OrderBy(i => i.Student.Surname).ThenBy(i => i.Student.Name).ThenBy(i => i.Student.Patronymic).ToList();
            }
            else
            {
                plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyName).WhereGroup(group));
            }

            DetailsGroupOfSpecialitiesVM model = new(group, plans, group.FormOfEducation.Year);

            return View(model);
        }

        public async Task<IActionResult> Create(string facultyName, int year)
        {
            CreateChangeGroupOfSpecVM model;
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties());
            if (faculty == null)
            {
                return NotFound();
            }
            GroupOfSpecialties group = new()
            {
                FormOfEducation = new()
                {
                    IsBudget = true,
                    IsDailyForm = true,
                    IsFullTime = true,
                    Year = year
                }
            };
            model = new CreateChangeGroupOfSpecVM()
            {
                Group = group,
                SelectedSpecialities = GetSelectedSpecialities(faculty, group),
                SelectedSubjects = await GetSelectedSubjectsAsync(group)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string facultyName, CreateChangeGroupOfSpecVM model)
        {
            if (ModelState.IsValid)
            {
                FormOfEducation form = new()
                {
                    IsDailyForm = model.Group.FormOfEducation.IsDailyForm,
                    IsBudget = model.Group.FormOfEducation.IsBudget,
                    IsFullTime = model.Group.FormOfEducation.IsFullTime,
                    Year = model.Group.FormOfEducation.Year,
                };
                form = _formsOfEducationRepository.GetAllAsync(new FormOfEducationSpecification().WhereForm(form)).Result.SingleOrDefault() ?? form;
                model.Group.FormOfEducation = form;
                model.Group.Specialities = await GetSelectedSpecialitiesFromModelAsync(model.SelectedSpecialities);
                model.Group.Subjects = await GetSelectedSubjectsFromModelAsync(model.SelectedSubjects);
                await _groupsOfSpecialtiesRepository.AddAsync(model.Group);
                _logger.LogInformation("Создана группа - {GroupName} - {Year} года на факультете - {FacultyName}", model.Group.Name, model.Group.FormOfEducation.Year, facultyName);

                return RedirectToAction("Details", "Faculties", new { name = facultyName });
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string facultyName, int id)
        {
            CreateChangeGroupOfSpecVM model;
            GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification(facultyName).IncludeSubjects().IncludeSpecialties());
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties());

            if (group == null || faculty == null)
            {
                return NotFound();
            }

            model = new CreateChangeGroupOfSpecVM()
            {
                Group = group,
                SelectedSpecialities = GetSelectedSpecialities(faculty, group),
                SelectedSubjects = await GetSelectedSubjectsAsync(group)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string facultyName, CreateChangeGroupOfSpecVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(model.Group.Id, new GroupsOfSpecialitiesSpecification(facultyName).IncludeSubjects().IncludeSpecialties());
                    if (group != null)
                    {
                        FormOfEducation form = new()
                        {
                            IsDailyForm = model.Group.FormOfEducation.IsDailyForm,
                            IsBudget = model.Group.FormOfEducation.IsBudget,
                            IsFullTime = model.Group.FormOfEducation.IsFullTime,
                            Year = model.Group.FormOfEducation.Year,
                        };
                        form = _formsOfEducationRepository.GetAllAsync(new FormOfEducationSpecification().WhereForm(form)).Result.SingleOrDefault() ?? form;

                        group.Name = model.Group.Name;
                        group.Description = model.Group.Description;
                        group.StartDate = model.Group.StartDate;
                        group.EnrollmentDate = model.Group.EnrollmentDate;
                        group.FormOfEducation = form;                        
                        group.Specialities = await GetSelectedSpecialitiesFromModelAsync(model.SelectedSpecialities);
                        group.Subjects = await GetSelectedSubjectsFromModelAsync(model.SelectedSubjects);

                        await _groupsOfSpecialtiesRepository.UpdateAsync(group);
                    }
                    _logger.LogInformation("Изменена группа - {GroupName} - {Year} года на факультете {FacultyName}", model.Group.Name, model.Group.FormOfEducation.Year, facultyName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await GroupOfSpecialtiesExists(model.Group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Faculties", new { name = facultyName });
            }
            return View(model);
        }

        public async Task<RedirectToActionResult> Delete(string facultyName, int id)
        {
            GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification());
            if (group != null)
            {
                await _groupsOfSpecialtiesRepository.DeleteAsync(id);
                _logger.LogInformation("Группа - {GroupName} - {Year} года на факультете - {FacultyName} - была удалена", group.Name, group.FormOfEducation.Year, facultyName);
            }

            return RedirectToAction("Details", "Faculties", new { name = facultyName, });
        }

        private async Task<bool> GroupOfSpecialtiesExists(int id)
        {
            var group = await _groupsOfSpecialtiesRepository.GetAllAsync();
            return group.Any(e => e.Id == id);
        }

        private static List<IsSelectedSpecialityInGroupVM> GetSelectedSpecialities(Faculty faculty, GroupOfSpecialties group)
        {
            List<IsSelectedSpecialityInGroupVM> isSelectedSpecialties = new();

            foreach (Speciality speciality in (faculty.Specialities ?? new List<Speciality>()).OrderBy(s => int.Parse(string.Join("", s.Code.Where(c => char.IsDigit(c))))))
            {
                Speciality? plan = (group.Specialities ?? new List<Speciality>()).Where(i => i.Equals(speciality)).SingleOrDefault();
                isSelectedSpecialties.Add(new(speciality, plan != null));
            }

            return isSelectedSpecialties;
        }

        private async Task<List<IsSelectedSubjectVM>> GetSelectedSubjectsAsync(GroupOfSpecialties group)
        {
            List<IsSelectedSubjectVM> isSelectedSubjects = new();

            foreach (Subject subject in (await _subjectsRepository.GetAllAsync()) ?? new List<Subject>())
            {
                Subject? isSubject = (group.Subjects ?? new List<Subject>()).Where(i => i.Equals(subject)).SingleOrDefault();
                isSelectedSubjects.Add(new(subject, isSubject != null));
            }

            return isSelectedSubjects;
        }

        private async Task<List<Speciality>> GetSelectedSpecialitiesFromModelAsync(List<IsSelectedSpecialityInGroupVM>? selectedSpecialities)
        {
            List<Speciality> specialities = new();

            if (selectedSpecialities != null)
            {
                foreach (IsSelectedSpecialityInGroupVM isSelectedSpecialty in selectedSpecialities.Where(p => p.IsSelected))
                {
                    Speciality? specialty = await _specialtiesRepository.GetByIdAsync(isSelectedSpecialty.SpecialityId);
                    if (specialty != null)
                    {
                        specialities.Add(specialty);
                    }
                }
            }

            return specialities;
        }

        private async Task<List<Subject>> GetSelectedSubjectsFromModelAsync(List<IsSelectedSubjectVM>? selectedSubjects)
        {
            List<Subject> subjects = new();

            if (selectedSubjects != null)
            {
                foreach (IsSelectedSubjectVM isSelectedSubject in selectedSubjects.Where(p => p.IsSelected))
                {
                    Subject? subject = await _subjectsRepository.GetByIdAsync(isSelectedSubject.SubjectId);
                    if (subject != null)
                    {
                        subjects.Add(subject);
                    }
                }
            }

            return subjects;
        }

        private static List<Admission> SearchAdmissions(string? searchStudents, List<Admission>? admissions)
        {
            admissions ??= new();

            if (searchStudents != null)
            {
                List<string> searchWords = searchStudents.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    admissions = admissions.Where(i => i.Student.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(admissions.Where(i => i.Student.Surname.ToLower().Contains(word.ToLower()))).Distinct()
                        .Union(admissions.Where(i => i.Student.Patronymic.ToLower().Contains(word.ToLower()))).Distinct()
                        .ToList();
                }
            }

            return admissions;
        }
    }
}
