namespace NewInterlex.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IUseCaseRequestHandler<in TUseCaseRequest, TUseCaseResponse> where TUseCaseRequest: IUseCaseRequest<TUseCaseResponse>
    {
        Task<TUseCaseResponse> Handle(TUseCaseRequest message);
    }
}