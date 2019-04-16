namespace NewInterlex.Core.Dto.UseCaseResponses
{
    using Interfaces;

    public class UcExchangeRefreshTokenResponse : UseCaseResponseMessage
    {
        public AccessToken AccessToken { get; set; }

        public string RefreshToken { get; set; }


        public UcExchangeRefreshTokenResponse(bool success = false, string message = null) : base(success, message)
        {
        }


        public UcExchangeRefreshTokenResponse(AccessToken accessToken, string refreshToken, bool success = false,
            string message = null) : base(success, message)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }
}