using Bilet_2.Models;
using Bilet_2.Models.Auth;
using Bilet_2.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bilet_2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.roleManager = roleManager;
        }


        public async Task< IActionResult> Register()
        {
    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateRegisterVM createRegister)
        {
            if (!ModelState.IsValid)
            {
                return View(createRegister);
            }
            AppUser user = new AppUser 
            {
                UserName=createRegister.Username,
                Email=createRegister.Email
            };
            IdentityResult regResult =await _userManager.CreateAsync(user,createRegister.Password);

          
            if (!regResult.Succeeded)
            {
                foreach (var error in regResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(createRegister);
            }
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
