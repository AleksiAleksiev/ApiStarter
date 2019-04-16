namespace NewInterlex.Core.Interfaces.Gateways.Repositories
{
    using System.Threading.Tasks;
    using Dto.GatewayResponses.Repositories;
    using Entities;

    public interface IUserRepository : IRepository<User>
    {
        Task<CreateUserResponse> Create(string email, string userName,
            string password);

        Task<User> FindByName(string userName);

        Task<bool> CheckPassword(User user, string password);
    }
}