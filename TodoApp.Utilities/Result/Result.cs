using System.Collections.Generic;

namespace TodoApp.Utilities
{
    public interface IResult
    {
        List<string> Errors { get; init; }
        bool IsFailure { get; }
        bool IsSuccess { get; init; }
    }

    public record Result : IResult
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public List<string> Errors { get; init; }

        public Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public Result(bool isSuccess, List<string> errors) : this(isSuccess)
        {
            Errors = errors;
        }

        public static Result Success()
            => new Result(isSuccess: true);

        public static Result Failure(List<string> errors)
            => new Result(isSuccess: false, errors);

        public static Result Failure(string error)
            => new Result(isSuccess: false, new List<string> { error });
    }
}
