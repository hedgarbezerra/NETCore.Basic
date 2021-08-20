using FluentValidation;
using FluentValidation.Results;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services.Validation
{
    public interface IUserValidation : IValidator<User>
    {
        void VerifyExistingUser(string username);
    }
    public class UserValidation : AbstractValidator<User>, IUserValidation
    {
        public IRepository<User> _repository { get; }
        public UserValidation(IRepository<User> repository)
        {
            _repository = repository;

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("E-mail invalid.");

        }

        public void VerifyExistingUser(string username)
        {
            throw new NotImplementedException();
        }

        new ValidationResult Validate(User user)
        {
            if (user is null) return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("E-mail", "User can't be null.") });
            // throw new ApplicationException("User can't be null");

            return base.Validate(user);
        }
    }
}
