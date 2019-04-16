namespace NewInterlex.Api.Validation
{
    using FluentValidation;
    using Models.Request;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            this.RuleFor(x => x.Username).NotEmpty();
            this.RuleFor(x => x.Password).NotEmpty();
        }
    }
}