namespace NewInterlex.Core.UseCases
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dto.UseCaseRequests;
    using Dto.UseCaseResponses;
    using Interfaces.Gateways.Repositories;
    using Interfaces.Services;
    using Interfaces.UseCases;

    public class ExchangeRefreshTokenUseCase : IExchangeRefreshTokenUseCase
    {
        private readonly IJwtTokenValidator jwtTokenValidator;
        private readonly IUserRepository userRepository;
        private readonly IJwtFactory jwtFactory;
        private readonly ITokenFactory tokenFactory;

        public ExchangeRefreshTokenUseCase(IJwtTokenValidator jwtTokenValidator, IUserRepository userRepository,
            IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            this.jwtTokenValidator = jwtTokenValidator;
            this.userRepository = userRepository;
            this.jwtFactory = jwtFactory;
            this.tokenFactory = tokenFactory;
        }

        public async Task<UcExchangeRefreshTokenResponse> Handle(UcExchangeRefreshTokenRequest message)
        {
            var claimsPrincipal = this.jwtTokenValidator.GetPrincipalFromToken(message.AccessToken, message.SigningKey);
            var response = new UcExchangeRefreshTokenResponse();
            if (claimsPrincipal != null)
            {
                var id = claimsPrincipal.Claims.First(x => x.Type == "id");
                var user = await this.userRepository.GetSingleBySpec(x => x.IdentityId == id.Value);
                if (user.HasValidRefreshToken(message.RefreshToken))
                {
                    var jwtToken = await this.jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                    var refreshToken = this.tokenFactory.GenerateToken();
                    user.RemoveRefreshToken(message.RefreshToken);
                    user.AddRefreshToken(refreshToken, user.Id);
                    await this.userRepository.Update(user);
                    response = new UcExchangeRefreshTokenResponse(jwtToken, refreshToken, true);
                    return response;
                }
            }

            response.Message = "Invalid token";
            return response;
        }
    }
}