namespace NewInterlex.Api.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Dto.UseCaseRequests;
    using Core.Interfaces.UseCases;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Request;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IRegisterUserUseCase registerUserUseCase;

        public AccountsController(IRegisterUserUseCase registerUserUseCase)
        {
            this.registerUserUseCase = registerUserUseCase;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var useCaseRequest = new UcRegisterUserRequest(request.Email, request.Username, request.Password);
            var response = await this.registerUserUseCase.Handle(useCaseRequest);
            var contentResult = new JsonContentResult
            {
                StatusCode = (int) (response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest),
                Content = JsonSerializer.SerializeObject(response)
            };
            
            return contentResult;

        }
    }
}