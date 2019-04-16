namespace NewInterlex.Core.Interfaces.Services
{
    using System.Threading.Tasks;
    using Dto;

    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}