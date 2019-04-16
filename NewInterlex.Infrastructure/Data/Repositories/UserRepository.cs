namespace NewInterlex.Infrastructure.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Core.Dto;
    using Core.Dto.GatewayResponses.Repositories;
    using Core.Entities;
    using Core.Interfaces.Gateways.Repositories;
    using Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    internal sealed class UserRepository : EfRepository<User>, IUserRepository
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(AppDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager) :
            base(dbContext)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<CreateUserResponse> Create(string email, string userName, string password)
        {
            var applicationUser = new ApplicationUser
            {
                Email = email,
                UserName = userName,
            };
            var identityResult = await this.userManager.CreateAsync(applicationUser, password);
            if (identityResult.Succeeded)
            {
                var user = new User(applicationUser.Id, userName, email);
                this.dbContext.Users.Add(user);
                await this.dbContext.SaveInfoAndChangesAsync();
                var response = new CreateUserResponse(applicationUser.Id, true);
                return response;
            }
            else
            {
                var response = new CreateUserResponse(applicationUser.Id, false,
                    identityResult.Errors.Select(x => new Error(x.Code, x.Description)));
                return response;
            }
        }

        public async Task<User> FindByName(string userName)
        {
            var applicationUser = await this.userManager.FindByNameAsync(userName);
            var user = await this.GetSingleBySpec(x => x.IdentityId == applicationUser.Id);
            return applicationUser != null ? this.mapper.Map(applicationUser, user) : null;
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            var applicationUser = this.mapper.Map<ApplicationUser>(user);
            var valid = await this.userManager.CheckPasswordAsync(applicationUser, password);
            return valid;
        }
    }
}