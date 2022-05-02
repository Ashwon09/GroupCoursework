using Microsoft.AspNetCore.Mvc;

namespace GroupCoursework.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
