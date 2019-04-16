namespace NewInterlex.Core.Dto.UseCaseResponses
{
    using System.Collections.Generic;
    using Interfaces;

    public class UcRegisterUserResponse : UseCaseResponseMessage
    {
        public string Id { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public UcRegisterUserResponse(IEnumerable<string> errors, bool success = false, string message = null) : base(
            success, message)
        {
            this.Errors = errors;
        }

        public UcRegisterUserResponse(string id, bool success = false, string message = null) : base(success, message)
        {
            this.Id = id;
        }
    }
}