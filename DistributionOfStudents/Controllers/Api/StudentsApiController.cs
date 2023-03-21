using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Students;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsApiController : ControllerBase
    {
        private readonly IAdmissionsRepository _admissionsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;

        public StudentsApiController(IAdmissionsRepository admissionsRepository, IRecruitmentPlansRepository plansRepository)
        {
            _admissionsRepository = admissionsRepository;
            _plansRepository = plansRepository;
        }

        // GET: api/ApiSubjects
        [HttpGet]
        public async Task<ActionResult<List<DetailsStudentVM>>> GetStudents(string? searchStudents, int page, int pageLimit)
        {
            DetailsStudentsVM model = new(new(), searchStudents);
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification());
            int lastYear = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;
            List<Admission> allAdmissions = await _admissionsRepository.SearchByStudentsAsync(searchStudents,
                new AdmissionsSpecification(i => i.GroupOfSpecialties.FormOfEducation.Year == lastYear).SortByStudent().IncludeGroupWithSpecialities());
            allAdmissions = allAdmissions.Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
            foreach (Admission admission in allAdmissions)
            {
                GroupOfSpecialties group = admission.GroupOfSpecialties;
                DetailsStudentVM detailsStudent;

                if (group.Specialities != null)
                {
                    string studentFullName = admission.Student.Surname + " " + admission.Student.Name + " " + admission.Student.Patronymic;
                    string facultyName = group.Specialities.First().Faculty.ShortName;
                    detailsStudent = new(studentFullName, facultyName, group);
                    model.Students.Add(detailsStudent);
                }
            }

            return model.Students;
        }

        [HttpGet("CountOfSearchStudents")]
        public async Task<ActionResult<int>> GetCountOfSearchStudentsAsync(string? searchStudents)
        {
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification());
            int lastYear = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;
            return new ActionResult<int>(_admissionsRepository.SearchByStudentsAsync(searchStudents, new AdmissionsSpecification(i => i.GroupOfSpecialties.FormOfEducation.Year == lastYear)).Result.Count);
        }
    }
}