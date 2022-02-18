using FluentValidation;
using FluentValidation.Results;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NETCore.Basic.Services.Validation
{
    public class UserValidation : AbstractValidator<User>, IValidator<User>
    {
        public IRepository<User> _repository { get; }
        public UserValidation(IRepository<User> repository)
        {
            _repository = repository;

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("E-mail invalid.");

            RuleFor(p => p.Username)
                .NotEmpty()
                .Must(VerifyExistingUser)
                .WithMessage("'Username' already exists.");

            RuleFor(p => p.Password)
                .NotEmpty()
                .Must(x => x.Length >= 8)
                .WithMessage((user, password) => $"'Password' minimum length is 8 not {password.Length}")
                .Must(ValidatePassword)
                .WithMessage("Password didn't meet the requirement for 1 special character, 1 upper case letter, 1 lower case letter and 1 number.");


        }

        private bool VerifyExistingUser(string username) => !_repository.Get(x => x.Username == username).ToList().Any();

        private bool ValidatePassword(string password) => Regex.IsMatch(password, @"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$");
        new ValidationResult Validate(User user)
        {
            if (user is null) return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("User", "User can't be null.") });

            return base.Validate(user);
        }
    }
}
