﻿using webapi.Data.Models;
using webapi.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using webapi.Data.Interfaces.Repositories;

namespace webapi.Controllers.Api
{
    [Route("api/{facultyName}/[controller]")]
    [ApiController]
    public class SpecialitiesApiController : ControllerBase
    {
        private readonly ILogger<SpecialitiesApiController> _logger;
        private readonly IFacultiesRepository _facultiesRepository;
        private readonly ISpecialitiesRepository _specialtiesRepository;

        public SpecialitiesApiController(ILogger<SpecialitiesApiController> logger, IFacultiesRepository facultiesRepository, ISpecialitiesRepository specialtiesRepository)
        {
            _logger = logger;
            _facultiesRepository = facultiesRepository;
            _specialtiesRepository = specialtiesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetSpecialities(string facultyName)
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
            return faculty.Specialities;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Speciality>> GetSpeciality(string facultyName, int id)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName);
            Speciality? speciality = await _specialtiesRepository.GetByIdAsync(id);
            if (faculty == null || speciality == null)
            {
                return NotFound();
            }

            return speciality;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpeciality(string facultyName, int id, Speciality speciality)
        {
            if (id != speciality.Id)
            {
                return BadRequest();
            }
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName);
            if (faculty == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
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

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
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

                return CreatedAtAction("GetSpeciality", new { id = speciality.Id }, speciality);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeciality(string facultyName, int id)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(facultyName);
            Speciality? specialty = await _specialtiesRepository.GetByIdAsync(id);
            if (faculty != null && specialty != null)
            {
                await _specialtiesRepository.DeleteAsync(id);
                _logger.LogInformation("Специальность - {SpecialityName} - была удалена", specialty.FullName);
            }

            return NoContent();
        }

        private async Task<bool> SpecialtyExists(int id)
        {
            var spec = await _specialtiesRepository.GetAllAsync();
            return spec.Any(e => e.Id == id);
        }
    }
}