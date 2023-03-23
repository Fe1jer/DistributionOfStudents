using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.RecruitmentPlans;
using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/[controller]/[action]")]
    public class RecruitmentPlansController : Controller
    {

        public RecruitmentPlansController()
        {
        }

        [Route("~/[controller]")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
