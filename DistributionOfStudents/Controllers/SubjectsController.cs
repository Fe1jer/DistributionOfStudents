using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers
{
    public class SubjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
