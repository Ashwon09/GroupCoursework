using GroupCoursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroupCoursework.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //FOR REGISTER page vierw
        public IActionResult RegisterStaff()
        {
            return View();
        }
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        //FOR REGISTER
        //
        [HttpPost]
        //FOR REGISTER STAFF AND ASSIGN ROLE
        public async Task<IActionResult> RegisterStaff(RegisterModel registermodel)
        {
            //check if incoming model object is valid
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registermodel.Email,
                };

                var result = await _userManager.CreateAsync(user, registermodel.Password);
                var RegisteredUser = user.Id;
                if (result.Succeeded)
                {
                    var userRole = new IdentityRole();
                 result = await _userManager.AddToRoleAsync(user, "Staff");
                    Console.WriteLine(result);
                    return RedirectToAction("Homepage", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(registermodel);
        }

        //FOR ADMIN RESGISTRER
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterModel registermodel)
        {
            //check if incoming model object is valid
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registermodel.Email,
                };

                var result = await _userManager.CreateAsync(user, registermodel.Password);
                var RegisteredUser = user.Id;
                if (result.Succeeded)
                {
                    var userRole = new IdentityRole();
                    result = await _userManager.AddToRoleAsync(user, "Admin");
                    Console.WriteLine(result);
                    return RedirectToAction("Homepage", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(registermodel);
        }

        //FOR LOGIN PAGE VIEW
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        //FOR LOGIN
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginmodel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginmodel.Email, loginmodel.Password, !loginmodel.RememberMe, false);//if lock account on failure
                //returnns sign in result
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError(string.Empty, "Invalid Username/Password");

            }
            return View(loginmodel);
        }


        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
