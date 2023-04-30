using webapi.Data.Models;
using webapi.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using webapi.Data.Interfaces.Repositories;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialitiesApiController : ControllerBase
    {
        private readonly ILogger<SpecialitiesApiController> _logger;
        private readonly IFacultiesRepository _facultiesRepository;
        private readonly ISpecialitiesRepository _specialtiesRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsOfSpecialtiesRepository;

        public SpecialitiesApiController(ILogger<SpecialitiesApiController> logger, IFacultiesRepository facultiesRepository,
            ISpecialitiesRepository specialtiesRepository, IGroupsOfSpecialitiesRepository groupsOfSpecialtiesRepository)
        {
            _logger = logger;
            _facultiesRepository = facultiesRepository;
            _specialtiesRepository = specialtiesRepository;
            _groupsOfSpecialtiesRepository = groupsOfSpecialtiesRepository;
        }

        [HttpGet("FacultySpecialities/{facultyName}")]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetFacultySpecialities(string facultyName)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeSpecialties());
            if (faculty == null)
            {
                return NotFound();
            }
            if (faculty.Specialities == null)
            {
                return NotFound();
            }

            faculty.Specialities.ForEach(i => i.Faculty = new());
            faculty.Specialities.ForEach(i => i.GroupsOfSpecialties = null);
            faculty.Specialities.ForEach(i => i.RecruitmentPlans = null);
            return faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToArray();
        }

        [HttpGet("FacultyDisabledSpecialities/{facultyName}")]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetFacultyDisabledSpecialities(string facultyName)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName, new FacultiesSpecification().IncludeDisabledSpecialties());
            if (faculty == null)
            {
                return NotFound();
            }
            if (faculty.Specialities == null)
            {
                return NotFound();
            }

            faculty.Specialities.ForEach(i => i.Faculty = new());
            faculty.Specialities.ForEach(i => i.GroupsOfSpecialties = null);
            faculty.Specialities.ForEach(i => i.RecruitmentPlans = null);
            return faculty.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToArray();
        }

        [HttpGet("GroupSpecialities/{groupId}")]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetGroupSpecialities(int groupId)
        {
            GroupOfSpecialties? group = await _groupsOfSpecialtiesRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties());
            if (group == null)
            {
                return NotFound();
            }
            if (group.Specialities == null)
            {
                return NotFound();
            }

            group.Specialities.ForEach(i => i.Faculty = new());
            group.Specialities.ForEach(i => i.GroupsOfSpecialties = null);
            group.Specialities.ForEach(i => i.RecruitmentPlans = null);
            return group.Specialities.OrderBy(sp => sp.DirectionCode ?? sp.Code).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Speciality>> GetSpeciality(int id)
        {
            Speciality? speciality = await _specialtiesRepository.GetByIdAsync(id);
            if (speciality == null)
            {
                return NotFound();
            }

            return speciality;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpeciality(int id, Speciality speciality)
        {
            if (id != speciality.Id)
            {
                return BadRequest();
            }
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(speciality.Faculty.ShortName);
            if (faculty == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    speciality.Faculty = faculty;
                    await _specialtiesRepository.UpdateAsync(speciality);
                    _logger.LogInformation("Изменена специальность - {SpecialityName}", speciality.FullName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SpecialtyExists(speciality.Id))
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

        [HttpPost("{facultyName}")]
        public async Task<ActionResult<Speciality>> PostSpeciality(string facultyName, Speciality speciality)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName);

            if (faculty == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                speciality.Faculty = faculty;
                await _specialtiesRepository.AddAsync(speciality);
                _logger.LogInformation("Создана специальность - {SpecialityName}", speciality.FullName);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeciality(int id)
        {
            Speciality? specialty = await _specialtiesRepository.GetByIdAsync(id);
            if (specialty != null)
            {
                await _specialtiesRepository.DeleteAsync(id);
                _logger.LogInformation("Специальность - {SpecialityName} - была удалена", specialty.FullName);
            }

            return Ok();
        }

        private async Task<bool> SpecialtyExists(int id)
        {
            var spec = await _specialtiesRepository.GetAllAsync();
            return spec.Any(e => e.Id == id);
        }
    }
}
