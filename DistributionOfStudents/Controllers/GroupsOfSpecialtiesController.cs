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
using Microsoft.CodeAnalysis;
using DistributionOfStudents.ViewModels;
using NuGet.Protocol.Plugins;
using System.Numerics;
using DistributionOfStudents.Data.Services;

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

        public GroupsOfSpecialtiesController(ILogger<GroupsOfSpecialtiesController> logger, IFacultiesRepository facultiesRepository,
            IGroupsOfSpecialitiesRepository groupsOfSpecialtiesRepository, ISpecialitiesRepository specialtiesRepository,
            IRecruitmentPlansRepository plansRepository, ISubjectsRepository subjectsRepository)
        {
            _logger = logger;
            _facultiesRepository = facultiesRepository;
            _groupsOfSpecialtiesRepository = groupsOfSpecialtiesRepository;
            _specialtiesRepository = specialtiesRepository;
            _plansRepository = plansRepository;
            _subjectsRepository = subjectsRepository;
        }

        [Route("~/Faculties/{facultyName}/{id}")]
        public async Task<IActionResult> Details(string facultyName, int id, string? searchStudents)
        {
            List<RecruitmentPlan> plans;
            DistributionService distributionService;
            GroupOfSpecialties group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }
            if (!group.IsCompleted)
            {
                plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
                distributionService = new(plans, group.Admissions);
                plans = distributionService.GetPlansWithEnrolledStudents();

                if (searchStudents != null)
                {
                    List<string> searchWords = searchStudents.Split(" ").ToList();
                    foreach (string word in searchWords)
                    {
                        group.Admissions = group.Admissions.Where(i => i.Student.Name.ToLower().Contains(word.ToLower())).ToList()
                            .Union(group.Admissions.Where(i => i.Student.Surname.ToLower().Contains(word.ToLower()))).Distinct()
                            .Union(group.Admissions.Where(i => i.Student.Patronymic.ToLower().Contains(word.ToLower()))).Distinct()
                            .ToList();
                    }
                }

                group.Admissions = group.Admissions.OrderBy(i => i.Student.Surname).ThenBy(i => i.Student.Name).ThenBy(i => i.Student.Patronymic).ToList();
            }
            else
            {
                plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyName).WhereGroup(group));
            }
           
            DetailsGroupOfSpecialitiesVM model = new() { GroupOfSpecialties = group, RecruitmentPlans = plans, FacultyShortName = facultyName, Year = group.Year };

            return View(model);
        }

        public async Task<IActionResult> Create(string facultyName, int year)
        {
            CreateChangeGroupOfSpecVM model;
            Faculty faculty = await _facultiesRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties());
            if(faculty == null)
            {
                return NotFound();
            }
            GroupOfSpecialties group = new()
            {
                IsBudget = true,
                IsDailyForm = true,
                IsFullTime = true,
                Year = year
            };
            model = new CreateChangeGroupOfSpecVM()
            {
                FacultyShortName = facultyName,
                Group = group,
                SelectedSpecialities = GetSelectedSpecialityAsync(faculty, group),
                SelectedSubjects = await GetSelectedSpeciality(group)
            };

            return View(model);
        }

        private static List<IsSelectedSpecialityInGroupVM> GetSelectedSpecialityAsync(Faculty faculty, GroupOfSpecialties group)
        {
            List<IsSelectedSpecialityInGroupVM> isSelectedSpecialties = new();

            foreach (Speciality specialty in faculty.Specialities ?? new List<Speciality>())
            {
                Speciality? plan = (group.Specialities ?? new List<Speciality>()).Where(i => i.Equals(specialty)).SingleOrDefault();
                IsSelectedSpecialityInGroupVM isSelectedSpecialty = new()
                {
                    SpecialityId = specialty.Id,
                    SpecialityName = specialty.DirectionName ?? specialty.FullName,
                    IsSelected = plan != null,
                };
                isSelectedSpecialties.Add(isSelectedSpecialty);
            }

            return isSelectedSpecialties;
        }

        private async Task<List<IsSelectedSubjectVM>> GetSelectedSpeciality(GroupOfSpecialties group)
        {
            List<IsSelectedSubjectVM> isSelectedSubjects = new();

            foreach (Subject subject in (await _subjectsRepository.GetAllAsync()) ?? new List<Subject>())
            {
                Subject? isSubject = (group.Subjects ?? new List<Subject>()).Where(i => i.Equals(subject)).SingleOrDefault();
                IsSelectedSubjectVM isSelectedSubject = new()
                {
                    SubjectId = subject.Id,
                    Subject = subject.Name,
                    IsSelected = isSubject != null,
                };
                isSelectedSubjects.Add(isSelectedSubject);
            }

            return isSelectedSubjects;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string facultyName, CreateChangeGroupOfSpecVM model)
        {
            if (ModelState.IsValid)
            {
                List<Speciality> recruitmentPlans = new();
                List<Subject> subjects = new();
                if (model.SelectedSpecialities != null)
                {
                    foreach (IsSelectedSpecialityInGroupVM isSelectedSpecialty in model.SelectedSpecialities.Where(p => p.IsSelected == true))
                    {
                        Task<Speciality> getSpecialty = _specialtiesRepository.GetByIdAsync(isSelectedSpecialty.SpecialityId);
                        recruitmentPlans.Add(await getSpecialty);
                    }
                }
                if (model.SelectedSubjects != null)
                {
                    foreach (IsSelectedSubjectVM isSelectedSubject in model.SelectedSubjects.Where(p => p.IsSelected == true))
                    {
                        Task<Subject> getSubject = _subjectsRepository.GetByIdAsync(isSelectedSubject.SubjectId);
                        subjects.Add(await getSubject);
                    }
                }
                model.Group.Year = model.Group.StartDate.Year;
                model.Group.Specialities = recruitmentPlans;
                model.Group.Subjects = subjects;
                await _groupsOfSpecialtiesRepository.AddAsync(model.Group);
                _logger.LogInformation("Создана группа - {GroupName} - {Year} года на факультете - {FacultyName}", model.Group.Name, model.Group.Year, facultyName);

                return RedirectToAction("Details", "Faculties", new { name = facultyName });
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string facultyName, int id)
        {
            CreateChangeGroupOfSpecVM model;
            GroupOfSpecialties group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification(facultyName).IncludeSubjects().IncludeSpecialties());
            Faculty faculty = await _facultiesRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties());

            if (group == null || faculty == null)
            {
                return NotFound();
            }

            model = new CreateChangeGroupOfSpecVM()
            {
                FacultyShortName = facultyName,
                Group = group,
                SelectedSpecialities = GetSelectedSpecialityAsync(faculty, group),
                SelectedSubjects = await GetSelectedSpeciality(group)
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
                    List<Speciality> recruitmentPlans = new();
                    List<Subject> subjects = new();
                    if (model.SelectedSpecialities != null)
                    {
                        foreach (IsSelectedSpecialityInGroupVM isSelectedSpecialty in model.SelectedSpecialities.Where(p => p.IsSelected == true))
                        {
                            Task<Speciality> getSpecialty = _specialtiesRepository.GetByIdAsync(isSelectedSpecialty.SpecialityId);
                            recruitmentPlans.Add(await getSpecialty);
                        }
                    }
                    if (model.SelectedSubjects != null)
                    {
                        foreach (IsSelectedSubjectVM isSelectedSubject in model.SelectedSubjects.Where(p => p.IsSelected == true))
                        {
                            Task<Subject> getSubject = _subjectsRepository.GetByIdAsync(isSelectedSubject.SubjectId);
                            subjects.Add(await getSubject);
                        }
                    }
                    GroupOfSpecialties group = await _groupsOfSpecialtiesRepository.GetByIdAsync(model.Group.Id, new GroupsOfSpecialitiesSpecification(model.FacultyShortName).IncludeSubjects().IncludeSpecialties());
                    group.Year = model.Group.StartDate.Year;
                    group.Name = model.Group.Name;
                    group.IsBudget = model.Group.IsBudget;
                    group.IsDailyForm = model.Group.IsDailyForm;
                    group.IsFullTime = model.Group.IsFullTime;
                    group.Description = model.Group.Description;
                    group.StartDate = model.Group.StartDate;
                    group.EnrollmentDate = model.Group.EnrollmentDate;
                    group.Specialities = recruitmentPlans;
                    group.Subjects = subjects;

                    await _groupsOfSpecialtiesRepository.UpdateAsync(group);
                    _logger.LogInformation("Изменена группа - {GroupName} - {Year} года на факультете {FacultyName}", model.Group.Name, model.Group.Year, facultyName);
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
            GroupOfSpecialties group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id);
            await _groupsOfSpecialtiesRepository.DeleteAsync(id);
            _logger.LogInformation("Группа - {GroupName} - {Year} года на факультете - {FacultyName} - была удалена", group.Name, group.Year, facultyName);

            return RedirectToAction("Details", "Faculties", new { name = facultyName, });
        }

        private async Task<bool> GroupOfSpecialtiesExists(int id)
        {
            var group = await _groupsOfSpecialtiesRepository.GetAllAsync();
            return group.Any(e => e.Id == id);
        }
    }
}
