using GroupCoursework.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroupCoursework.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;        }
        public IActionResult Index()
        {
            return View();
        }

        //FOR ROLE CREATE
        public async Task<IActionResult> CreateRoleAdmin()
        {
            //if (ModelState.IsValid)
           // {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin",

                };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok();
                }


           // }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoleStaff()
        {
            //if (ModelState.IsValid)
            // {
            IdentityRole role = new IdentityRole
            {
                Name = "Staff",

            };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok();
            }


            // }

            return RedirectToAction("Index", "Home");
        }
    }
}
