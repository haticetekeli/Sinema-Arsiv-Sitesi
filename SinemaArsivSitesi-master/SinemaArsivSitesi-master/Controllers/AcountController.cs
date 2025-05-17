using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SinemaArsivSitesi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SinemaArsivSitesi.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult UserSelect() => View();

        [HttpGet]
        public IActionResult Login() => View();

        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public IActionResult AdminLogin() => View();

        [HttpGet]
        public IActionResult AdminRegister() => View();

        [HttpGet]
        public IActionResult AdminLoginRegister() => View();

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail)
                        ?? await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Json(new { success = true, redirect = Url.Action("Index", "Home") });
            }

            return Json(new { success = false, message = "Geçersiz kullanıcı adı veya şifre!" });
        }

        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, false);
                return Json(new { success = true, redirect = Url.Action("Login", "User") });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = $"Kayıt sırasında hata oluştu: {errors}" });
        }

        [HttpPost]
        public async Task<JsonResult> AdminLogin([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail)
                        ?? await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user != null &&
                await _userManager.CheckPasswordAsync(user, model.Password) &&
                await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Json(new { success = true, redirect = Url.Action("Index", "Home") });
            }

            return Json(new { success = false, message = "Geçersiz yönetici bilgileri veya yetkiniz yok!" });
        }

        [HttpPost]
        public async Task<JsonResult> AdminRegister([FromBody] RegisterViewModel model)
        {
            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                await _signInManager.SignInAsync(user, false);
                return Json(new { success = true, redirect = Url.Action("AdminLogin", "Account") });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = $"Kayıt sırasında hata oluştu: {errors}" });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
