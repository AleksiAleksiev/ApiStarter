namespace NewInterlex.Core.Dto.UseCaseResponses
{
    using System.Collections.Generic;
    using Interfaces;

    public class UcLoginResponse : UseCaseResponseMessage
    {
        public AccessToken AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public IEnumerable<Error> Errors { get; set; }
        
        public UcLoginResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public UcLoginResponse(AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}