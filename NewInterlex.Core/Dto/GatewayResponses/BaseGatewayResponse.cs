namespace NewInterlex.Core.Dto.GatewayResponses
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract class BaseGatewayResponse
    {
        public bool Success { get; set; }

        public IEnumerable<Error> Errors { get; set; }

        protected BaseGatewayResponse(bool success = false, IEnumerable<Error> errors = null)
        {
            this.Success = success;
            this.Errors = errors;
        }
    }
}