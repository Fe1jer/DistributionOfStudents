using webapi.Data.Models;
using webapi.Data.Specifications;
using webapi.ViewModels.Students;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces.Repositories;

namespace webapi.Controllers.Api
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

        [HttpGet]
        public async Task<ActionResult<object>> GetStudents(string? searchStudents, int page, int pageLimit)
        {
            List<DetailsStudentVM> models = new();
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification());
            int lastYear = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;
            List<Admission> allAdmissions = await _admissionsRepository.SearchByStudentsAsync(searchStudents, new AdmissionsSpecification(i => i.GroupOfSpecialties.FormOfEducation.Year == lastYear).SortByStudent().IncludeGroupWithSpecialities());
            int countOfSearchStudents = allAdmissions.Count;
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
                    models.Add(detailsStudent);
                }
            }

            return new { students = models, countOfSearchStudents };
        }
    }
}