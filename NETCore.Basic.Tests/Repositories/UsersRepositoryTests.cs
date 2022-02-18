using Microsoft.EntityFrameworkCore;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Repository.DataContext;
using NUnit.Framework;
using System;
using System.Linq;

namespace NETCore.Basic.Repository.Repositories.Tests
{
    [TestFixture()]
    public class UsersRepositoryTests
    {
        private NetDbContext _context;
        private UsersRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NetDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            _context = new NetDbContext(options);
            _repo = new UsersRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.ChangeTracker.Clear();
            _context?.Database.EnsureDeleted();
        }

        [Test]
        public void Get_AllUsers_ReturnsUsers()
        {
            AddTwoUsers();
            var users = _repo.Get();

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);
            Assert.That(users.Count() >= 2);
        }

        [Test]
        public void Get_AllUsers_EmptyIqueryable()
        {
            var users = _repo.Get();

            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Get_FilteredById_ReturnsSingleUser(int id)
        {
            AddTwoUsers();
            var user = _repo.Get(id);

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Id, id);
        }

        [Test]
        [TestCase(5)]
        [TestCase(8)]
        public void Get_FilteredById_ReturnsNull(int id)
        {
            AddTwoUsers();
            var users = _repo.Get(id);

            Assert.IsNull(users);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-5)]
        public void Get_FilteredByInvalidId_ThrowsArgumentException(int id)
        {
            Assert.Throws<ArgumentException>(() => _repo.Get(id));
        }

        [Test]
        public void Get_FilteredByDelegate_ReturnsFilteredUsers()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt < DateTime.Now);

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);
            Assert.That(users.Count() >= 2);
        }

        [Test]
        public void Get_FilteredByDelegate_ReturnsEmptyQueryable()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt > DateTime.Now);

            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
        }

        [Test]
        public void Get_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsOldestToNewest()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt < DateTime.Now, c => c.RegistredAt);
            var userOld = users.First();

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 1);
        }

        [Test]
        public void Get_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsNewestToOldest()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt < DateTime.Now, c => c.RegistredAt, reverse: true);
            var userOld = users.First();

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 2);
        }

        [Test]
        public void Get_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsNoRecord()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt < DateTime.Now, c => c.RegistredAt, 1, 1, true);
            var userOld = users.FirstOrDefault();

            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
            Assert.IsNull(userOld);
        }

        [Test]
        public void Get_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsSingeRecord()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt < DateTime.Now, c => c.RegistredAt, 2, 1, true);
            var user = users.FirstOrDefault();

            Assert.IsNotNull(users);
            Assert.IsNotEmpty(users);
            Assert.IsNotNull(user);
            Assert.That(user.Id == 1);
        }

        [Test]
        public void Get_FilteredAndOrderedWithNoMatches_ReturnsEmpty()
        {
            AddTwoUsers();
            var users = _repo.Get(x => x.RegistredAt > DateTime.Now, c => c.RegistredAt);

            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_DeletingUsers_UserDeletedFromContext(int id)
        {
            AddTwoUsers();

            var user = _repo.Get(id);
            _repo.Delete(user);
            _repo.SaveChanges();

            Assert.That(!_context.Users.Any(x => x.Id == id));
        }

        [Test]
        public void GetDbContext_ReturnsCurrentDbContext()
        {
            var dbContext = _repo.GetDbContext();

            Assert.IsNotNull(dbContext);
        }

        [Test]
        public void Add_UserAdded()
        {
            var user = new User { Name = "New user", Username = "roleUser", RegistredAt = new DateTime(2021, 08, 29) };
            var addedUser = _repo.Add(user);
            _repo.SaveChanges();
            var users = _repo.Get();
            Assert.That(users.Count() == 1);
            Assert.AreEqual(users.First(), addedUser);
        }

        [Test]
        public void Update_()
        {
            AddTwoUsers();

            var user = _repo.Get(1);
            user.Role = Role.Administrator;
            user.Name = "New User";

            var updatedUser = _repo.Update(user);
            _repo.SaveChanges();

            var users = _repo.Get();

            Assert.That(users.Count() == 2);
            Assert.IsNotNull(updatedUser);
            Assert.IsTrue(user.Id == updatedUser.Id);
            Assert.IsTrue(updatedUser.Role == Role.Administrator);
        }

        private void AddTwoUsers()
        {
            _context.Users.Add(new User { Id = 1, Name = "User 1", RegistredAt = new DateTime(2021, 02, 19), Role = Role.Common });
            _context.Users.Add(new User { Id = 2, Name = "User 2", RegistredAt = new DateTime(2021, 05, 29), Role = Role.Administrator });
            _context.SaveChanges();
        }
    }
}