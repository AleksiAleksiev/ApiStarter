namespace NewInterlex.Core.Dto.UseCaseRequests
{
    using Interfaces;
    using UseCaseResponses;

    public class UcLoginRequest : IUseCaseRequest<UcLoginResponse>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public UcLoginRequest(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}