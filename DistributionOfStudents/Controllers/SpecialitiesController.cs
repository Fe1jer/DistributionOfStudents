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

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/[controller]/[action]")]
    public class SpecialitiesController : Controller
    {
        private readonly ILogger<SpecialitiesController> _logger;
        private readonly IFacultiesRepository _facultiesRepository;
        private readonly ISpecialitiesRepository _specialtiesRepository;

        public SpecialitiesController(ILogger<SpecialitiesController> logger, IFacultiesRepository facultiesRepository, ISpecialitiesRepository specialtiesRepository)
        {
            _logger = logger;
            _facultiesRepository = facultiesRepository;
            _specialtiesRepository = specialtiesRepository;
        }

        // GET: Specialties/Create
        public async Task<IActionResult> Create(string facultyName)
        {
            Faculty faculty = (await _facultiesRepository.GetAllAsync(new FacultiesSpecification().WhereShortName(facultyName))).Single();
            Speciality specialty = new()
            {
                Faculty = faculty
            };

            return View(specialty);
        }

        // POST: Specialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Speciality specialty)
        {
            if (ModelState.IsValid)
            {
                Faculty faculty = (await _facultiesRepository.GetAllAsync(new FacultiesSpecification().WhereShortName(specialty.Faculty.ShortName))).Single();

                specialty.Faculty = faculty;
                await _specialtiesRepository.AddAsync(specialty);
                _logger.LogInformation("Создана специальность - {SpecialityName}", specialty.FullName);

                return RedirectToAction("Details", "Faculties", new { name = specialty.Faculty.ShortName });
            }
            return View(specialty);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(string facultyName, int id)
        {
            Speciality specialty = await _specialtiesRepository.GetByIdAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            Faculty faculty = (await _facultiesRepository.GetAllAsync(new FacultiesSpecification().WhereShortName(facultyName))).Single();
            specialty.Faculty = faculty;
            return View(specialty);
        }

        // POST: Specialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string facultyName, int id, Speciality specialty)
        {
            if (id != specialty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _specialtiesRepository.UpdateAsync(specialty);
                    _logger.LogInformation("Изменена специальность - {SpecialityName}", specialty.FullName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SpecialtyExists(specialty.Id))
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
            return View(specialty);
        }

        // GET: FacultiesController/Delete/5
        public async Task<RedirectToActionResult> Delete(string facultyName, int id)
        {
            Speciality specialty = await _specialtiesRepository.GetByIdAsync(id);
            await _specialtiesRepository.DeleteAsync(id);
            _logger.LogInformation("Специальность - {SpecialityName} - была удалена", specialty.FullName);

            return RedirectToAction("Details", "Faculties", new { name = facultyName, });
        }

        private async Task<bool> SpecialtyExists(int id)
        {
            var spec = await _specialtiesRepository.GetAllAsync();
            return spec.Any(e => e.Id == id);
        }
    }
}
