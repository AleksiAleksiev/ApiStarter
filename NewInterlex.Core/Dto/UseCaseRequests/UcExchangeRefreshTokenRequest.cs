namespace NewInterlex.Core.Dto.UseCaseRequests
{
    using Interfaces;
    using UseCaseResponses;

    public class UcExchangeRefreshTokenRequest : IUseCaseRequest<UcExchangeRefreshTokenResponse>
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string SigningKey { get; set; }

        public UcExchangeRefreshTokenRequest(string accessToken, string refreshToken, string signingKey)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.SigningKey = signingKey;
        }
    }
}