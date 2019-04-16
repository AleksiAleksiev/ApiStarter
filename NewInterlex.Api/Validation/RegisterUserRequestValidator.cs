namespace NewInterlex.Api.Validation
{
    using FluentValidation;
    using Models.Request;

    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            this.RuleFor(x => x.Email).EmailAddress();
            this.RuleFor(x => x.Username).Equal(x => x.Email); // are we gonna have separate Username?
            this.RuleFor(x => x.Password).Length(6, 15);
//            this.RuleFor(x => x.Password).Matches(""); // for regex use this
        }
    }
}