using FluentValidation;
using System.Linq;

namespace TodoApp.Utilities
{
    public static class FluentValidationExtensions
    {
        public static Result Validation<TModel>(this IValidator<TModel> validator, TModel model)
        {
            if (model is null)
            {
                return Result.Failure("No model provided");
            }

            var result = validator.Validate(model);

            if (result.IsValid)
            {
                return Result.Success();
            }
            else
            {
                return Result.Failure(
                    result.Errors.Select(error => error.ErrorMessage).ToList());
            }
        }
    }
}
