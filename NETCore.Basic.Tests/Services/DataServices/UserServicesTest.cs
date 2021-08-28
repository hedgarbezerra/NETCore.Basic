using AutoMapper;
using FluentValidation.Results;
using Moq;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
using NETCore.Basic.Services.Data;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Services.Validation;
using NETCore.Basic.Util.Crypto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NETCore.Basic.Tests.Services.DataServices
{
    [TestFixture]
    public class UserServicesTest
    {
        private UserServices _userServices;
        private Mock<IValidator<User>> _mqValidationRules = new Mock<IValidator<User>>();
        private Mock<IRepository<User>> _mqRepository = new Mock<IRepository<User>>();
        private Mock<IHashing> _mqHashingService = new Mock<IHashing>();
        private Mock<IMapper> _mqMapper = new Mock<IMapper>();

        [SetUp]
        public void Setup()
        {
            _userServices = new UserServices(_mqValidationRules.Object, _mqRepository.Object, _mqHashingService.Object, _mqMapper.Object);
            _mqHashingService.Setup(c => c.ComputeHash(It.IsAny<string>())).Returns("qweuhqwuieopklqwe");
        }

        [Test]
        public void Add_WithValidUser_RegisterUser()
        {
            var user = new User() { Username = "username", Role = Role.Administrator, Password = "password", Id = 0 };

            _mqValidationRules.Setup(c => c.Validate(user)).Returns(new ValidationResult());
            _mqRepository.Setup(c => c.Add(user)).Returns(new User { Username = "username", Role = Role.Administrator, Password = "password", Id = 1 });

            var result = _userServices.Add(user, out List<ValidationFailure> resultList);

            Assert.IsTrue(result);
            Assert.That(resultList.Count <=0);
        }

        [Test]
        public void Add_WithInvalidUser_ReturnFalse()
        {
            var user = new User() { Username = "username", Role = Role.Administrator, Password = "password", Id = 0 };
            var erros = new List<ValidationFailure>() 
            {
                new ValidationFailure("Email", "ERRO 1"),
                new ValidationFailure("Email", "ERRO 2")
            };

            _mqValidationRules.Setup(c => c.Validate(user)).Returns(new ValidationResult(erros));

            var result = _userServices.Add(user, out List<ValidationFailure> resultList);
            Assert.IsFalse(result);
            Assert.That(resultList.Count > 0);

        }
        
        [Test]
        public void Authenticate_PasswordValidated_ReturnsTrue()
        {
            var user = new User() { Username = "username", Role = Role.Administrator, Password = "password" };
            var userList = new List<User>() { user };
            _mqRepository.Setup(g => g.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => userList.AsQueryable());
            _mqHashingService.Setup(c => c.VerifyHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _userServices.Authenticate(user, out User outUser);

            Assert.IsTrue(result);
        }

        [Test]
        public void Authenticate_PasswordIncorrect_ReturnsFalse()
        {
            var user = new User() { Username = "username", Role = Role.Administrator, Password = "password" };
            var userList = new List<User>() { user };
            _mqRepository.Setup(g => g.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => userList.AsQueryable());
            _mqHashingService.Setup(c => c.VerifyHash(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = _userServices.Authenticate(user, out User outUser);

            Assert.IsFalse(result);
        }

        [Test]
        public void Authenticate_UserNotFound_ReturnsFalse()
        {
            var user = new User() { Username = "username", Role = Role.Administrator, Password = "password" };
            var userList = new List<User>();
            _mqRepository.Setup(g => g.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => userList.AsQueryable());

            var result = _userServices.Authenticate(user, out User outUser);

            Assert.IsFalse(result);
        }

        public void Get()
        {

        }
        public void Gety()
        {
        }
        public void Geta() 
        {
        }
        public void GetHateoas() 
        {
        }
        public void Update()
        {

        }

        [Test]
        [TestCase(1)]
        [TestCase(6)]
        public void Delete_UserNotFound_ThrowsException(int id)
        {
            Assert.Throws<Exception>(() => _userServices.Delete(id));
        }

        [Test]
        [TestCase("Users", 1, 1)]
        [TestCase("Users", 2, 2)]
        public void GetPaginatedList_ReturnsPaginatedList(string route, int index, int size)
        {
            var list = new List<OutputUser>
            {
                new OutputUser { Id = 1,  Email = "", Name = "", Role = Role.Administrator },
                new OutputUser { Id = 2,  Email = "", Name = "", Role = Role.Manager },
                new OutputUser { Id = 3,  Email = "", Name = "", Role = Role.Common },
                new OutputUser { Id = 4,  Email = "", Name = "", Role = Role.Manager },
                new OutputUser { Id = 5,  Email = "", Name = "", Role = Role.Administrator },

            };
            _mqMapper.Setup(r => r.ProjectTo<OutputUser>(It.IsAny<IQueryable<User>>(), typeof(OutputUser))).Returns(list.AsQueryable());

            var paginatedResult = _userServices.GetPaginatedList(new Mock<IUriService>().Object, route, index, size);

            Assert.AreEqual(paginatedResult.Data.Count, size);
            Assert.AreEqual(paginatedResult.PageSize, size);
            Assert.IsTrue(paginatedResult.HasNextPage);
            Assert.That(paginatedResult.TotalCount == list.Count);
            Assert.That(paginatedResult.TotalPages == (int)Math.Ceiling(paginatedResult.TotalCount / (double)size));
        }

        [Test]
        public void GetPaginatedList_EmptyQueryable_ReturnsPaginatedList()
        {
            var list = new List<User>();

            _mqRepository.Setup(r => r.Get()).Returns(list.AsQueryable());
            int index = 1, size = 1;

            var paginatedResult = _userServices.GetPaginatedList(new Mock<IUriService>().Object, "", index, size);

            Assert.AreEqual(paginatedResult.Data.Count, 0);
            Assert.AreEqual(paginatedResult.PageSize, size);
            Assert.That(paginatedResult.TotalCount == list.Count);
            Assert.That(paginatedResult.TotalPages == (int)Math.Ceiling(paginatedResult.TotalCount / (double)size));

        }
    }
}
