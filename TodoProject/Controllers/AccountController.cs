using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
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
            if (ModelState.IsValid)
            {
                string hashedPassword = DoMD5HashedString(model.Password);

                User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);
                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError("", "Kullanıcı hesabınız kilitlenmiştir. Lütfen yöneticinizle iletişime geçin.");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? String.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Username", user.Username));

                    ClaimsIdentity Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(Identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Todo");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                }



            }
            return View(model);
        }

        private string DoMD5HashedString(string s)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5Salt;
            string hashed = salted.MD5();
            return hashed;
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
            if (ModelState.IsValid)
            {
                if(_databaseContext.Users.Any(x => x.Username == model.Username))
                {
                    ModelState.AddModelError(nameof(model.Username), "Bu kullanıcı adı zaten alınmış. Lütfen farklı bir kullanıcı adı deneyin.");
                   return View(model);

                }
                string hashedPassword = DoMD5HashedString(model.Password);

                User user = new User
                {
                    Username = model.Username,  
                    Password = hashedPassword
                };
                _databaseContext.Users.Add(user);
               int affectedRowCount= _databaseContext.SaveChanges();
                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "Kayıt işlemi başarısız oldu. Lütfen tekrar deneyin.");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }

            }
            return View(model);
        }
        public IActionResult Profile()
        {
            ProfileInfoLoader();

            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

            ViewData["FullName"] = user.FullName;
        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required] [StringLength(50)] string? fullname)
        {

            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
               User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);
               user.FullName = fullname;
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Profile));   
            }
            ProfileInfoLoader();
            return View("Profile");
        }
        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {

            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                string hashedPassword = DoMD5HashedString(password);
                user.Password = hashedPassword;
                _databaseContext.SaveChanges();

                TempData["PasswordChanged"] = true; //  toastr için işaret veriyoruz
                return RedirectToAction("Profile");
            }
            ProfileInfoLoader();
            return View("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
