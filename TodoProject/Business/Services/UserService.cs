using TodoProject.Business.Interfaces;
 using TodoProject.Entities;
using TodoProject.Models;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TodoProject.Business.Services
{

    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UserService(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool RegisterUser(RegisterViewModel model)
        {
            if (_context.Users.Any(x => x.Username == model.Username))
            {
                return false; // Kullanıcı adı zaten var
            }

            string hashedPassword = DoMD5HashedString(model.Password);

            User user = new User
            {
                Username = model.Username,
                Password = hashedPassword,
            };
            _context.Users.Add(user);
            int affected = _context.SaveChanges();

            return affected > 0; // kayıt başarılıysa true
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

            User? user = _context.Users
                .SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);

            if (user == null || user.Locked)
            {
                return null;
            }
            return user;
        }
        public User? GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);





        }

        public bool UpdateUser(Guid userId, string fullName)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return false;
            user.FullName = fullName;
            _context.SaveChanges();
            return true;
        }
        public bool UpdatePassword(Guid userId, string newPassword)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return false;
            string hashedPassword = DoMD5HashedString(newPassword);
            user.Password = hashedPassword;
            _context.SaveChanges();
            return true;

        }
        public string? GetFullName(Guid userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId)?.FullName;
        }
    }
}