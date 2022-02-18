using FluentValidation.Results;

namespace NETCore.Basic.Services.Validation
{
    public interface IValidator<T> where T : class
    {
        ValidationResult Validate(T obj);
    }
}
