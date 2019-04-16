using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NewInterlex.Core.Entities;
using NewInterlex.Infrastructure.Identity;

namespace NewInterlex.Infrastructure.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<string> CreateUser(string email, string userName, string password) // change response model and maybe parameters
        {
            var appUser = new ApplicationUser { Email = email, UserName = userName };
            var identityResult = await this.userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
            {
                return "error error";
            }

            //var user = new User(firstName, lastName, appUser.Id, appUser.UserName); // create entity 
            //_appDbContext.Users.Add(user);

            //await _appDbContext.SaveChangesAsync();

            return "error error";
        }
    }
}
