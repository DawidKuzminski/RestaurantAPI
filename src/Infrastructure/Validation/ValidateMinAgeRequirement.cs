using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Infrastructure.Validation;
public class ValidateMinAgeRequirement : IAuthorizationRequirement
{
    public int MinAge { get; init; }

    public ValidateMinAgeRequirement(int minAge)
    {
        MinAge = minAge;
    }
}

public class ValidateMinAgeRequirementHandler : AuthorizationHandler<ValidateMinAgeRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidateMinAgeRequirement requirement)
	{
        var dateOfBirth = DateTime.Parse(context.User.FindFirst(x => x.Type == "DateOfBirth").Value);
        if(dateOfBirth.AddYears(requirement.MinAge) < DateTime.Today)
            context.Succeed(requirement);

        return Task.CompletedTask;
	}
}

