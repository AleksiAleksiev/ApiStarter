namespace NewInterlex.Core.Dto.GatewayResponses.Repositories
{
    using System.Collections.Generic;

    public class CreateUserResponse : BaseGatewayResponse
    {
        public string Id { get; set; }

        public CreateUserResponse(string id, bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
            this.Id = id;
        }
    }
}