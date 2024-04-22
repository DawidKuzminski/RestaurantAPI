using FluentValidation;
using RestaurantAPI.Core.DTO;

namespace RestaurantAPI.Infrastructure.Validation;
public class RestaurantGetAllQueryValidator : AbstractValidator<RestaurantGetAllQuery>
{
    private int[] allowedPageSizes = new int[] { 10, 50, 100};

    public RestaurantGetAllQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).Custom((value, context) =>
        {
            if (!allowedPageSizes.Contains(value))
                context.AddFailure("PageSize", $"PageSize must be in: [{string.Join(",", allowedPageSizes)}]");
        });
    }
}
