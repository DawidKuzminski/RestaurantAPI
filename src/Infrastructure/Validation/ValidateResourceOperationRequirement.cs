using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Core.Entity;
using System.Security.Claims;

namespace RestaurantAPI.Infrastructure.Validation;

public class ValidateResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation ResourceOperation { get; set; }

    public ValidateResourceOperationRequirement(ResourceOperation resourceOperation)
    {
        ResourceOperation = resourceOperation;
    }
}

public class ValidateResourceOperationRequirementHandler : AuthorizationHandler<ValidateResourceOperationRequirement, RestaurantEntity>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidateResourceOperationRequirement requirement, RestaurantEntity resource)
    {
        if (requirement.ResourceOperation == ResourceOperation.Read ||
            requirement.ResourceOperation == ResourceOperation.Create)
			context.Succeed(requirement);

		var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        if(resource.OwnerId == int.Parse(userId))
			context.Succeed(requirement);

        return Task.CompletedTask;
	}
}

public enum ResourceOperation
{
    Create,
    Read,
    Update,
    Delete
}
