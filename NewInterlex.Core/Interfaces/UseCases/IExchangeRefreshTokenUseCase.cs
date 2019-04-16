namespace NewInterlex.Core.Interfaces.UseCases
{
    using Dto.UseCaseRequests;
    using Dto.UseCaseResponses;

    public interface IExchangeRefreshTokenUseCase : IUseCaseRequestHandler<UcExchangeRefreshTokenRequest, UcExchangeRefreshTokenResponse>
    {
        
    }
}