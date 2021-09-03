using NUnit.Framework;
using NETCore.Basic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NETCore.Basic.Repository.DataContext;
using Microsoft.EntityFrameworkCore;
using NETCore.Basic.Domain.Entities;
using System.Linq;
using NETCore.Basic.Domain.Interfaces;

namespace NETCore.Basic.Repository.Repositories.Tests
{
    [TestFixture()]
    public class LogRepositoryTests
    {
        private NetDbContext _context;
        private LogRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NetDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            _context = new NetDbContext(options);
            _repo = new LogRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.ChangeTracker.Clear();
            _context?.Database.EnsureDeleted();
        }

        [Test]
        public void Get_AllEventLogs_ReturnsEventLogs()
        {
            AddLogsToContext();
            var logs = _repo.Get();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.That(logs.Count() >= 2);
        }

        [Test]
        public void Get_AllEventLogs_EmptyIqueryable()
        {
            var logs = _repo.Get();

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Get_FilteredById_ReturnsSingleEventLog(int id)
        {
            AddLogsToContext();
            var log = _repo.Get(id);

            Assert.IsNotNull(log);
            Assert.AreEqual(log.Id, id);
        }

        [Test]
        [TestCase(5)]
        [TestCase(8)]
        public void Get_FilteredById_ReturnsNull(int id)
        {
            AddLogsToContext();
            var logs = _repo.Get(id);

            Assert.IsNull(logs);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-5)]
        public void Get_FilteredByInvalidId_ThrowsArgumentException(int id)
        {
            Assert.Throws<ArgumentException>(() => _repo.Get(id));
        }

        [Test]
        public void Get_FilteredByDelegate_ReturnsFilteredEventLogs()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now);

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.That(logs.Count() >= 2);
        }

        [Test]
        public void Get_FilteredByDelegate_ReturnsEmptyQueryable()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime > DateTime.Now);

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
        }

        [Test]
        public void Get_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsOldestToNewest()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime);
            var userOld = logs.First();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 1);
        }

        [Test]
        public void Get_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsNewestToOldest()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, reverse: true);
            var userOld = logs.First();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 2);
        }

        [Test]
        public void Get_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsNoRecord()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, 1, 1, true);
            var userOld = logs.FirstOrDefault();

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
            Assert.IsNull(userOld);
        }

        [Test]
        public void Get_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsSingeRecord()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, 2, 1, true);
            var log = logs.FirstOrDefault();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(log);
            Assert.That(log.Id == 1);
        }

        [Test]
        public void Get_FilteredAndOrderedWithNoMatches_ReturnsEmpty()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime > DateTime.Now, c => c.CreatedTime);

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_DeletingEventLogs_EventLogDeletedFromContext(int id)
        {
            AddLogsToContext();

            var log = _repo.Get(id);
            _repo.Delete(log);
            _repo.SaveChanges();

            Assert.That(!_context.Logs.Any(x => x.Id == id));
        }

        [Test]
        public void GetDbContext_ReturnsCurrentDbContext()
        {
            var dbContext = _repo.GetDbContext();

            Assert.IsNotNull(dbContext);
        }

        [Test]
        public void Add_EventLogAdded()
        {
            var log = new EventLog { Message = "New log", Exception = "roleEventLog", CreatedTime = new DateTime(2021, 08, 29) };
            var addedEventLog = _repo.Add(log);
            _repo.SaveChanges();
            var logs = _repo.Get();
            Assert.That(logs.Count() == 1);
            Assert.AreEqual(logs.First(), addedEventLog);
        }


        private void AddLogsToContext()
        {
            _context.Logs.Add(new EventLog { Id = 1, Message = "Error 1", CreatedTime = new DateTime(2021, 5, 10)});
            _context.Logs.Add(new EventLog { Id = 2, Message = "Error 2", CreatedTime = new DateTime(2021, 8, 10) });
            _context.SaveChanges();
        }
    }
}