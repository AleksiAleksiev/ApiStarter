namespace NewInterlex.Core.UseCases
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dto.UseCaseRequests;
    using Dto.UseCaseResponses;
    using Interfaces.Gateways.Repositories;
    using Interfaces.UseCases;

    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository userRepository;

        public RegisterUserUseCase(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        public async Task<UcRegisterUserResponse> Handle(UcRegisterUserRequest message)
        {
            var response = await this.userRepository.Create(message.Email, message.UserName, message.Password);
            UcRegisterUserResponse regResponse;
            if (response.Success)
            {
                regResponse = new UcRegisterUserResponse(response.Id, true);
            }
            else
            {
                regResponse = new UcRegisterUserResponse(response.Errors.Select(x => x.Description));
            }

            return regResponse;
        }
    }
}