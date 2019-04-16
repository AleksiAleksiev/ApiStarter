namespace NewInterlex.Core.Interfaces
{
    public abstract class UseCaseResponseMessage
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        protected UseCaseResponseMessage(bool success = false, string message = null)
        {
            this.Success = success;
            this.Message = message;
        }
    }
}