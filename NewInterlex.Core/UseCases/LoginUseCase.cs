namespace NewInterlex.Core.UseCases
{
    using System.Threading.Tasks;
    using Dto;
    using Dto.UseCaseRequests;
    using Dto.UseCaseResponses;
    using Interfaces.Gateways.Repositories;
    using Interfaces.Services;
    using Interfaces.UseCases;

    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtFactory jwtFactory;
        private readonly ITokenFactory tokenFactory;

        public LoginUseCase(IUserRepository userRepository, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            this.userRepository = userRepository;
            this.jwtFactory = jwtFactory;
            this.tokenFactory = tokenFactory;
        }

        public async Task<UcLoginResponse> Handle(UcLoginRequest message)
        {
            var valid = !string.IsNullOrWhiteSpace(message.UserName) && !string.IsNullOrWhiteSpace(message.Password);
            if (valid)
            {
                var user = await this.userRepository.FindByName(message.UserName);
                if (user != null)
                {
                    var passwordValid = await this.userRepository.CheckPassword(user, message.Password);
                    if (passwordValid)
                    {
                        var refreshToken = this.tokenFactory.GenerateToken();
                        user.AddRefreshToken(refreshToken, user.Id);
                        await this.userRepository.Update(user);

                        var accessToken = await this.jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                        return new UcLoginResponse(accessToken, refreshToken, true);
                    }
                }
            }

            var error = new Error("login_failure", "Invalid username or password.");
            return new UcLoginResponse(new[] {error});
        }
    }
}