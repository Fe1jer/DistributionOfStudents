using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistributionOfStudents.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectsRepository _subjectsRepository;

        public SubjectsController(ISubjectsRepository subjectsRepository)
        {
            _subjectsRepository = subjectsRepository;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            return View(await _subjectsRepository.GetAllAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (await _subjectsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            Subject? subject = await _subjectsRepository
                .GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                await _subjectsRepository.AddAsync(subject);

                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (await _subjectsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }
            Subject? subject = await _subjectsRepository.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _subjectsRepository.UpdateAsync(subject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SubjectExists(subject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (await _subjectsRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subjects'  is null.");
            }
            Subject? subject = await _subjectsRepository.GetByIdAsync(id);
            if (subject != null)
            {
                await _subjectsRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubjectExists(int id)
        {
            return (await _subjectsRepository.GetAllAsync()).Any(e => e.Id == id);
        }
    }
}
