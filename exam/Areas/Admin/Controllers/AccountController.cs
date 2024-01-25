using exam.Areas.Admin.ViewModels;
using exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _usermanage;
        private readonly SignInManager<AppUser> _signmanage;
        private readonly RoleManager<IdentityRole> _rolemanage;

        public AccountController(UserManager<AppUser>usermanage,SignInManager<AppUser> signmanage,RoleManager<IdentityRole> rolemanage)
        {
            _usermanage = usermanage;
            _signmanage = signmanage;
            _rolemanage = rolemanage;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser user = new AppUser
            {
                Email=vm.Email,
                Name=vm.Name,
                Surname=vm.Surname,
                UserName=vm.UserName
            };
            var result = await _usermanage.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                }
            }
            await _signmanage.SignInAsync(user, false);
            return RedirectToAction("Index","Home",new {area=""});   
                
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginVM vm)
        {
            if(!ModelState.IsValid) return View(vm);
            AppUser user = await _usermanage.FindByNameAsync(vm.UserNameOrEmail);
            if (user == null)
            {
                user = await _usermanage.FindByEmailAsync(vm.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "UserName , Email or Password incorrect");
                }
                return View(vm);
            }
            var result = await _signmanage.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
            if(result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Accoun locked,pleace try again a few minutes");
                return View(vm);

            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "UserName , Email or Password incorrect");

                return View(vm);
            }
            await _signmanage.SignInAsync(user, false);
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signmanage.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> CreateRole()
        {
            await _rolemanage.CreateAsync(new IdentityRole
            {
                Name="Admin"
            });
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
