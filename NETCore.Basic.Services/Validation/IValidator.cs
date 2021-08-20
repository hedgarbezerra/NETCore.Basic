using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services.Validation
{
    public interface IValidator<T> where T: class
    {
        ValidationResult Validate(T obj);
    }
}
