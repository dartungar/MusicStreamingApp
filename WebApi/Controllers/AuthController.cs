using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Service;
using Service.DTO;

namespace WebApp.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _service;

        public AuthController(AuthService service) : base()
        {
            _service = service;
            _service._authenticateFunc = Authenticate;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _service.Login(loginDto);
                return Redirect("/");
            }
            catch (Exception)
            {
                // если неправильный пароль, давать нормальную обратную связь об этом
                TempData["Error"] = "Authorization error. Please try again or contact support@treolan.ru";
                return View();
            }
            
        }

        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            try
            {
                await _service.Register(userDto);
                TempData["Success"] = $"Welcome, {userDto.Name}!";
                return Redirect("/");
            }
            catch
            {
                TempData["Error"] = "Error while registering. Please try again or contact support@treolan.ru";
                return View();
            }
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
