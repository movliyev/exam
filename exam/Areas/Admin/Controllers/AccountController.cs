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
                    ModelState.AddModelError(String.Empty,)
                }
            }
                
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
