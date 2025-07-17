using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TodoProject.Business.Interfaces;
using TodoProject.Models;

namespace TodoProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var user = _userService.LoginUser(model);

            if (user == null) {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? String.Empty));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));
            claims.Add(new Claim("Username", user.Username));

            ClaimsIdentity Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(Identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Todo");

        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }



        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)

                return View(model);

            bool isRegistered = _userService.RegisterUser(model);
            if (!isRegistered)
            {
                ModelState.AddModelError(nameof(model.Username), "Bu kullanıcı adı zaten alınmış. Lütfen farklı bir kullanıcı adı deneyin.");
                return View(model);
            }

            return RedirectToAction(nameof(Login));


        }

            [HttpGet]
            public IActionResult Profile()
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIdStr == null)

                    return RedirectToAction(nameof(Login));
                var user = _userService.GetById(Guid.Parse(userIdStr));
            ViewData["FullName"] = user.FullName;
                return View(user);

            }







            [HttpPost]

            public IActionResult ProfileChangeFullName(string fullName)
            {

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if ((userIdStr == null))
                    return RedirectToAction(nameof(Login));

                bool updated = _userService.UpdateUser(Guid.Parse(userIdStr), fullName);
                if (!updated)
                {
                    ModelState.AddModelError(nameof(fullName), "Ad ve soyad güncellenemedi. Lütfen tekrar deneyin.");
                    var user = _userService.GetById(Guid.Parse(userIdStr));
                    ViewData["FullName"] = user.FullName;
                    return View("Profile", user);
                }
                return RedirectToAction(nameof(Profile));

            }




            [HttpPost]
            public IActionResult ProfileChangePassword(string password)
            {

                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIdStr == null)
                    return RedirectToAction(nameof(Login));

                bool updated = _userService.UpdatePassword(Guid.Parse(userIdStr), password);


                if (!updated)

                {
                    ModelState.AddModelError(nameof(password), "Şifre güncellenemedi. Lütfen tekrar deneyin.");
                    var user = _userService.GetById(Guid.Parse(userIdStr));
                    ViewData["FullName"] = user.FullName;
                    return View("Profile", user);
                }
                TempData["PasswordChanged"] = true; // toastr için işaret veriyoruz
                return RedirectToAction(nameof(Profile));
            }
        }

    }





