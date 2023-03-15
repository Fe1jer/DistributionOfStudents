using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Students;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace DistributionOfStudents.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IAdmissionsRepository _admissionsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;

        public StudentsController(IAdmissionsRepository admissionsRepository, IRecruitmentPlansRepository plansRepository)
        {
            _admissionsRepository = admissionsRepository;
            _plansRepository = plansRepository;
        }

        // GET: StudentsController
        public async Task<IActionResult> Index(string? searchStudents)
        {
            DetailsStudentsVM model = new(new(), searchStudents);
            List<RecruitmentPlan> allPlans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification());
            int lastYear = allPlans.Count != 0 ? allPlans.Max(i => i.FormOfEducation.Year) : 0;
            List<Admission> allAdmissions = await _admissionsRepository.GetAllAsync(new AdmissionsSpecification(i => i.GroupOfSpecialties.FormOfEducation.Year == lastYear).IncludeGroupWithSpecialities());
            allAdmissions = SearchAdmissions(searchStudents, allAdmissions).OrderBy(i => i.Student.Surname).ThenBy(i => i.Student.Name).ThenBy(i => i.Student.Patronymic).ToList();

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

            return View(model);
        }

        private static List<Admission> SearchAdmissions(string? searchStudents, List<Admission>? admissions)
        {
            admissions ??= new();

            if (searchStudents != null)
            {
                List<string> searchWords = searchStudents.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    admissions = admissions.Where(i => i.Student.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(admissions.Where(i => i.Student.Surname.ToLower().Contains(word.ToLower()))).Distinct()
                        .Union(admissions.Where(i => i.Student.Patronymic.ToLower().Contains(word.ToLower()))).Distinct()
                        .ToList();
                }
            }

            return admissions;
        }
    }
}
