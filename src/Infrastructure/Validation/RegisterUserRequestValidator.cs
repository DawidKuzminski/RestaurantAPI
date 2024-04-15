using FluentValidation;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Database;

namespace RestaurantAPI.Infrastructure.Validation;
public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator(RestaurantDbContext dbContext)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(50)
            .Custom((value, context) =>
            {
                var isEmailAlreadyExist = dbContext.Users.Any(x => x.Email == value);
                if (isEmailAlreadyExist)
                    context.AddFailure("Email", "Email already exist.");
			});

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);


    }
}
