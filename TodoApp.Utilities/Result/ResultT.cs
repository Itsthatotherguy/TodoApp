using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Utilities
{
    public interface IResult<TValue> : IResult
    {
        TValue Value { get; init; }
    }

    public record Result<TValue> : IResult<TValue>
    {
        public TValue Value { get; init; }
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public List<string> Errors { get; init; }

        public Result(bool isSuccess, TValue value)
        {
            IsSuccess = isSuccess;
            Value = value;
        }

        public Result(bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result<TValue> Success(TValue value)
            => new Result<TValue>(isSuccess: true, value);

        public static Result<TValue> Failure(List<string> errors)
            => new Result<TValue>(isSuccess: false, errors);

        public static Result<TValue> Failure(string error)
            => new Result<TValue>(isSuccess: false, new List<string> { error });
    }
}
