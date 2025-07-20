using TodoProject.Business.Interfaces;
 using TodoProject.Entities;
using TodoProject.Models;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TodoProject.Data.Interfaces;

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