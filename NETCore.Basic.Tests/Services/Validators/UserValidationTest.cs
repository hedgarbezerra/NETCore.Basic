using FluentValidation.TestHelper;
using Moq;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Services.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NETCore.Basic.Tests.Services.Validators
{
    /*    
     Documentação da validação e teste 
    https://docs.fluentvalidation.net/en/latest/testing.html
     */
    [TestFixture]
    public class UserValidationTest
    {
        private UserValidation validator;
        private Mock<IRepository<User>> mqRepository;

        [SetUp]
        public void Setup()
        {
            mqRepository = new Mock<IRepository<User>>();
            validator = new UserValidation(mqRepository.Object);
        }

        [Test]
        public void Validate_ValidUser_ValidationValidWithNoErrors()
        {
            var user = new User { Id = 1, Username = "User 1", Email = "hueheu@hotmail.com", Password = "@!913iHeda",  RegistredAt = new DateTime(2021, 02, 19) };
            var result = validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_InvalidEmail_ValidationValidWithNoErrors()
        {

            var user = new User { Id = 1, Username = "User 1", Email = "huehetmail.com", Password = "@!913iHeda", RegistredAt = new DateTime(2021, 02, 19) };
            var result = validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Username);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.Email);
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_EmptyUsername_ValidationValidWithNoErrors()
        {

            var user = new User { Id = 1, Username = "", Email = "hueheu@hotmail.com", Password = "@!913iHeda", RegistredAt = new DateTime(2021, 02, 19) };
            var result = validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.Username);
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_ExistingUser_ValidationValidWithNoErrors()
        {
            var listUser = new List<User>
            {
                new User { Id = 1, Username = "User1", RegistredAt = new DateTime(2021, 02, 19) },
                new User { Id = 2, Username = "User2", RegistredAt = new DateTime(2021, 05, 19) }
            };
            var user = new User { Username = "User1", Email = "hueheu@hotmail.com", Password = "@!913iHeda", RegistredAt = new DateTime(2021, 02, 19) };

            mqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(listUser.Where(x => x.Username == user.Username).AsQueryable());

            var result = validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.Username);
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_InvalidPassword_ValidationValidWithNoErrors()
        {
            var user = new User { Username = "XUser16", Email = "hueheu@hotmail.com", Password = "1235678", RegistredAt = new DateTime(2021, 02, 19) };
            var result = validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Password didn't meet the requirement for 1 special character, 1 upper case letter, 1 lower case letter and 1 number.");
            Assert.IsFalse(result.IsValid);

        }
        [Test]
        public void Validate_MinimumPasswordLengthNotMatched_ShouldHave()
        {
            var user = new User { Username = "XUser16", Email = "hueheu@hotmail.com", Password = "12345", RegistredAt = new DateTime(2021, 02, 19) };
            var result = validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage($"'Password' minimum length is 8 not {user.Password.Length}");
            Assert.IsFalse(result.IsValid);

        }
    }
}
