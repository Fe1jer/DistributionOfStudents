using Microsoft.AspNetCore.Mvc;

namespace DistributionOfStudents.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
