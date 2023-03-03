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

        public GroupsOfSpecialtiesController(ILogger<GroupsOfSpecialtiesController> logger, IFacultiesRepository facultiesRepository,
            IGroupsOfSpecialitiesRepository groupsOfSpecialtiesRepository, ISpecialitiesRepository specialtiesRepository, IRecruitmentPlansRepository plansRepository)
        {
            _logger = logger;
            _facultiesRepository = facultiesRepository;
            _groupsOfSpecialtiesRepository = groupsOfSpecialtiesRepository;
            _specialtiesRepository = specialtiesRepository;
            _plansRepository = plansRepository;
        }

        [Route("~/Faculties/{facultyName}/{id}")]
        public async Task<IActionResult> Details(string facultyName, int id)
        {
            GroupOfSpecialties group = (await _groupsOfSpecialtiesRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(id).IncludeAdmissions().IncludeSpecialties())).Single();
            if (group == null)
            {
                return NotFound();
            }

            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            plans = plans.Where(p => group.Specialities.Contains(p.Speciality)).ToList();
            DetailsGroupOfSpecialitiesVM model = new() { GroupOfSpecialties = group, RecruitmentPlans = plans };

            return View(model);
        }

        public async Task<IActionResult> Create(string facultyName, int year)
        {
            List<IsSelectedSpecialityInGroupVM> isSelectedSpecialties = new();
            CreateChangeGroupOfSpecVM model;
            Faculty faculty = (await _facultiesRepository.GetAllAsync(new FacultiesSpecification().WhereShortName(facultyName).IncludeSpecialties())).Single();
            GroupOfSpecialties group = new()
            {
                IsBudget = true,
                IsDailyForm = true,
                IsFullTime = true,
                Year = year
            };

            foreach (Speciality specialty in faculty.Specialities)
            {
                IsSelectedSpecialityInGroupVM isSelectedSpecialty = new()
                {
                    SpecialityId = specialty.Id,
                    SpecialityName = specialty.DirectionName ?? specialty.FullName,
                    IsSelected = false
                };
                isSelectedSpecialties.Add(isSelectedSpecialty);
            }
            model = new CreateChangeGroupOfSpecVM()
            {
                FacultyShortName = facultyName,
                Group = group,
                SelectedSpecialities = isSelectedSpecialties
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string facultyName, CreateChangeGroupOfSpecVM model)
        {
            if (ModelState.IsValid)
            {
                List<Speciality> recruitmentPlans = new();
                if (model.SelectedSpecialities != null)
                {
                    foreach (IsSelectedSpecialityInGroupVM isSelectedSpecialty in model.SelectedSpecialities.Where(p => p.IsSelected == true))
                    {
                        Task<Speciality> getSpecialty = _specialtiesRepository.GetByIdAsync(isSelectedSpecialty.SpecialityId);
                        recruitmentPlans.Add(await getSpecialty);
                    }
                }
                model.Group.Year = model.Group.StartDate.Year;
                model.Group.Specialities = recruitmentPlans;
                await _groupsOfSpecialtiesRepository.AddAsync(model.Group);
                _logger.LogInformation("Создана группа - {GroupName} - {Year} года на факультете - {FacultyName}", model.Group.Name, model.Group.Year, facultyName);

                return RedirectToAction("Details", "Faculties", new { name = facultyName });
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string facultyName, int id)
        {
            CreateChangeGroupOfSpecVM model;
            List<IsSelectedSpecialityInGroupVM> isSelectedSpecialties = new();
            Faculty faculty = (await _facultiesRepository.GetAllAsync(new FacultiesSpecification().WhereShortName(facultyName).IncludeSpecialties())).Single();
            GroupOfSpecialties group = (await _groupsOfSpecialtiesRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(id).IncludeSpecialties())).Single();

            foreach (Speciality specialty in faculty.Specialities)
            {
                Speciality? plan = group.Specialities.Where(i => i.Equals(specialty)).SingleOrDefault();
                IsSelectedSpecialityInGroupVM isSelectedSpecialty = new()
                {
                    SpecialityId = specialty.Id,
                    SpecialityName = specialty.DirectionName ?? specialty.FullName,
                    IsSelected = plan != null,
                };
                isSelectedSpecialties.Add(isSelectedSpecialty);
            }
            model = new CreateChangeGroupOfSpecVM()
            {
                FacultyShortName = facultyName,
                Group = group,
                SelectedSpecialities = isSelectedSpecialties
            };
            if (group == null)
            {
                return NotFound();
            }
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
                    if (model.SelectedSpecialities != null)
                    {
                        foreach (IsSelectedSpecialityInGroupVM isSelectedSpecialty in model.SelectedSpecialities.Where(p => p.IsSelected == true))
                        {
                            Task<Speciality> getSpecialty = _specialtiesRepository.GetByIdAsync(isSelectedSpecialty.SpecialityId);
                            recruitmentPlans.Add(await getSpecialty);
                        }
                    }
                    GroupOfSpecialties group = (await _groupsOfSpecialtiesRepository.GetAllAsync(new GroupsOfSpecialitiesSpecification(model.Group.Id).IncludeSpecialties())).Single();
                    group.Year = model.Group.StartDate.Year;
                    group.Name = model.Group.Name;
                    group.IsBudget = model.Group.IsBudget;
                    group.IsDailyForm = model.Group.IsDailyForm;
                    group.IsFullTime = model.Group.IsFullTime;
                    group.Description = model.Group.Description;
                    group.StartDate = model.Group.StartDate;
                    group.EnrollmentDate = model.Group.EnrollmentDate;
                    group.Specialities = recruitmentPlans;

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
