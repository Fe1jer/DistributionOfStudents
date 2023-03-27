using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Specifications;
using webapi.ViewModels.GroupsOfSpecialities;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsOfSpecialtiesApiController : ControllerBase
    {
        private readonly ILogger<GroupsOfSpecialtiesApiController> _logger;
        private readonly IGroupsOfSpecialitiesRepository _groupsOfSpecialtiesRepository;
        private readonly ISpecialitiesRepository _specialtiesRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IFormsOfEducationRepository _formsOfEducationRepository;

        public GroupsOfSpecialtiesApiController(ILogger<GroupsOfSpecialtiesApiController> logger,
            IGroupsOfSpecialitiesRepository groupsOfSpecialtiesRepository, ISpecialitiesRepository specialtiesRepository,
            ISubjectsRepository subjectsRepository, IFormsOfEducationRepository formsOfEducationRepository)
        {
            _logger = logger;
            _groupsOfSpecialtiesRepository = groupsOfSpecialtiesRepository;
            _specialtiesRepository = specialtiesRepository;
            _subjectsRepository = subjectsRepository;
            _formsOfEducationRepository = formsOfEducationRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupOfSpecialties>> GetGroup(int id)
        {
            GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        [HttpPost("{facultyName}")]
        public async Task<IActionResult> Create(string facultyName, CreateChangeGroupOfSpecVM model)
        {
            if (ModelState.IsValid)
            {
                FormOfEducation form = _formsOfEducationRepository.GetAllAsync(new FormOfEducationSpecification().WhereForm(model.Group.FormOfEducation)).Result.SingleOrDefault() ?? model.Group.FormOfEducation;

                model.Group.FormOfEducation = form;
                model.Group.Specialities = await GetSelectedSpecialitiesFromModelAsync(model.SelectedSpecialities);
                model.Group.Subjects = await GetSelectedSubjectsFromModelAsync(model.SelectedSubjects);
                await _groupsOfSpecialtiesRepository.AddAsync(model.Group);
                _logger.LogInformation("Создана группа - {GroupName} - {Year} года на факультете - {FacultyName}", model.Group.Name, model.Group.FormOfEducation.Year, facultyName);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{facultyName}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string facultyName, CreateChangeGroupOfSpecVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(model.Group.Id, new GroupsOfSpecialitiesSpecification().IncludeSubjects().IncludeSpecialties());
                    if (group != null)
                    {
                        FormOfEducation form = _formsOfEducationRepository.GetAllAsync(new FormOfEducationSpecification().WhereForm(model.Group.FormOfEducation)).Result.SingleOrDefault() ?? model.Group.FormOfEducation;

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
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{facultyName}/{id}")]
        public async Task<IActionResult> Delete(string facultyName, int id)
        {
            GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification());
            if (group != null)
            {
                await _groupsOfSpecialtiesRepository.DeleteAsync(id);
                _logger.LogInformation("Группа - {GroupName} - {Year} года на факультете - {FacultyName} - была удалена", group.Name, group.FormOfEducation.Year, facultyName);
            }

            return Ok();
        }

        private async Task<bool> GroupOfSpecialtiesExists(int id)
        {
            var group = await _groupsOfSpecialtiesRepository.GetAllAsync();
            return group.Any(e => e.Id == id);
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
    }
}
