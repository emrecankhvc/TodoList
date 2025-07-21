using System.Security.Claims;
using TodoProject.Entities;
using TodoProject.Models;
using System.Security.Claims;


namespace TodoProject.Business.Interfaces
{
    public interface IUserService
    {
        bool RegisterUser(RegisterViewModel model);
        User? LoginUser(LoginViewModel model);

        User? GetById(Guid id);
        bool UpdateUser(Guid userId,string fullName);
        bool UpdatePassword(Guid userId, string newPassword);

        ClaimsPrincipal CreateClaimsPrincipal(User user);
    }
       
}
