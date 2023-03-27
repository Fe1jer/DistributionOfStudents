using webapi.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using webapi.Data.Interfaces.Repositories;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsApiController : ControllerBase
    {
        private readonly ISubjectsRepository _subjectsRepository;

        public SubjectsApiController(ISubjectsRepository subjectsRepository)
        {
            _subjectsRepository = subjectsRepository;
        }

        // GET: api/ApiSubjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            if (await _subjectsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }
            return await _subjectsRepository.GetAllAsync();
        }

        // GET: api/ApiSubjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
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

            return subject;
        }

        // PUT: api/ApiSubjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
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

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject([FromBody]Subject subject)
        {
            if (await _subjectsRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subjects'  is null.");
            }
            if (ModelState.IsValid)
            {
                await _subjectsRepository.AddAsync(subject);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/ApiSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
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

            return Ok();
        }

        private async Task<bool> SubjectExists(int id)
        {
            return (await _subjectsRepository.GetAllAsync()).Any(e => e.Id == id);
        }
    }
}
