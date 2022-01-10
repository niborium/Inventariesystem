using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GammaltGlimmer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GammaltGlimmer.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController(
                   UserManager<AppUser> userManager,
                   SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var user =
               await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);

            if (user == null)
            {
                RedirectToAction("Login");
            }
            //login functionality

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AppUser appUser)
        {
            //login functionality
            var user = await _userManager.FindByNameAsync(appUser.UserName);

            if (user != null)
            {
                //sign in
                var signInResult = await _signInManager.PasswordSignInAsync
                (user, appUser.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Register");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            //register functionality

            var user = new AppUser
            {
                Id = "101",
                UserName = appUser.UserName,
                Email = appUser.Email,
                Name = appUser.Name,
                Password = appUser.Password
            };

            var result = await _userManager.CreateAsync(user, user.Password);

            if (result.Succeeded)
            {
                // User sign
                // sign in
                var signInResult = await _signInManager.PasswordSignInAsync
                (user, user.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public async Task<IActionResult> LogOut(string username, string password)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}