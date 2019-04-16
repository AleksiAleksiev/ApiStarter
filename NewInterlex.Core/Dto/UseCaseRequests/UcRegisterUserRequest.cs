namespace NewInterlex.Core.Dto.UseCaseRequests
{
    using Interfaces;
    using UseCaseResponses;

    public class UcRegisterUserRequest : IUseCaseRequest<UcRegisterUserResponse>
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public UcRegisterUserRequest(string email, string userName, string password)
        {
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
        }
    }
}