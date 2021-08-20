using Microsoft.EntityFrameworkCore;
using Moq;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Repository.DataContext;
using NETCore.Basic.Util.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCore.Basic.Tests.Repository
{
    [TestFixture]
    public class UsersRepositoryTest
    {
        private NetDbContext _context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NetDbContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

            _context = new NetDbContext(options, new Mock<IAPIConfigurations>().Object);
            _context.Users.Add(new User { Id = 1, Name = "Movie 1" });
            _context.Users.Add(new User { Id = 2, Name = "Movie 2" });
            _context.Users.Add(new User { Id = 3, Name = "Movie 3" });
            _context.SaveChanges();

        }

        [Test]
        public void Get_ReturnThreeUsers()
        {
            Assert.AreEqual(_context.Set<User>().Count(), 3);
        }
    }
}
