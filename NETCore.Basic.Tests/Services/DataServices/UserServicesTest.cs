using AutoMapper;
using Moq;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Services.Data;
using NETCore.Basic.Services.Validation;
using NETCore.Basic.Util.Crypto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        }


    }
}
