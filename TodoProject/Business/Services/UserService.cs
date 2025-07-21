using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;
using TodoProject.Business.Interfaces;
using TodoProject.Data.Interfaces;
 using TodoProject.Entities;
using TodoProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TodoProject.Business.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        public bool RegisterUser(RegisterViewModel model)
        {
            if (_userRepository.GetByUsername(model.Username) != null)
            {
                return false; // Kullanıcı adı zaten var
            }

            string hashedPassword = DoMD5HashedString(model.Password);

            User user = new User
            {
                Username = model.Username,
                Password = hashedPassword,
            };

            _userRepository.Add(user);
           _userRepository.Save();

            return true; // kayıt başarılıysa true
        }
        private string DoMD5HashedString(string s)
        {
            string salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + salt;
            string hashed = salted.MD5();
            return hashed;

        }

        public User? LoginUser(LoginViewModel model)
        {
            string hashedPassword = DoMD5HashedString(model.Password);

            var user = _userRepository.GetByUsername(model.Username);

            if (user == null || user.Password != hashedPassword || user.Locked)
            {
                return null;
            }
            return user;
        }

        public ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("Username", user.Username),
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public User? GetById(Guid id)
        {
            return _userRepository.GetById(id);

        }

        public bool UpdateUser(Guid userId, string fullName)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                return false;


            user.FullName = fullName;
            _userRepository.Update(user);
            _userRepository.Save();
            return true;
        }
        public bool UpdatePassword(Guid userId, string newPassword)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                return false;
            string hashedPassword = DoMD5HashedString(newPassword);
            user.Password = hashedPassword;
            _userRepository.Update(user);
            _userRepository.Save();
            return true;

        }
        public string? GetFullName(Guid userId)
        {
            return _userRepository.GetFullName(userId);
        }
    }
}