namespace NewInterlex.Api.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Dto.UseCaseRequests;
    using Core.Interfaces.UseCases;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.Request;
    using Models.Response;
    using Models.Settings;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUseCase loginUseCase;
        private readonly IExchangeRefreshTokenUseCase exchangeRefreshTokenUseCase;
        private readonly AuthSettings authSettings;

        public AuthController(ILoginUseCase loginUseCase, IExchangeRefreshTokenUseCase exchangeRefreshTokenUseCase,
            IOptions<AuthSettings> authSettings)
        {
            this.loginUseCase = loginUseCase;
            this.exchangeRefreshTokenUseCase = exchangeRefreshTokenUseCase;
            this.authSettings = authSettings.Value;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var useCaseRequest = new UcLoginRequest(request.Username, request.Password);
            var result = await this.loginUseCase.Handle(useCaseRequest);
            var contentResult = new JsonContentResult
            {
                StatusCode = (int) (result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest),
                Content = result.Success
                    ? JsonSerializer.SerializeObject(new TokensResponse(result.AccessToken, result.RefreshToken))
                    : JsonSerializer.SerializeObject(result.Errors)
            };
            return contentResult;
        }

        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var useCaseRequest = new UcExchangeRefreshTokenRequest(request.AccessToken, request.RefreshToken,
                this.authSettings.SecretKey);
            var result = await this.exchangeRefreshTokenUseCase.Handle(useCaseRequest);
            var contentResult = new JsonContentResult
            {
                StatusCode = (int) (result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest),
                Content = result.Success
                    ? JsonSerializer.SerializeObject(new TokensResponse(result.AccessToken, result.RefreshToken))
                    : JsonSerializer.SerializeObject(result.Message)
            };
            return contentResult;
        }
    }
}