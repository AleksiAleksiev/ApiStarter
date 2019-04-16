namespace NewInterlex.Api.Validation
{
    using FluentValidation;
    using Models.Request;

    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            this.RuleFor(x => x.AccessToken).NotEmpty();
            this.RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}