using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Util.Helper;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace NETCore.Basic.Services.Data
{
    public interface ILoggingService
    {
        IQueryable<EventLog> Get();
        IQueryable<EventLog> Get(Expression<Func<EventLog, bool>> filter);
        EventLog Get(int id);
        PaginatedList<EventLog> GetPaginatedList(string route, int pageIndex, int pageSize);
        bool DeleteFileLogs(string folderPath);

    }
    public class LoggingService : ILoggingService
    {
        public LoggingService(IRepository<EventLog> repository, IUriService uriService, ILocalFileHandler fileHandler)
        {
            _repository = repository;
            _uriService = uriService;
            _fileHandler = fileHandler;
        }

        public IRepository<EventLog> _repository { get; }
        public IUriService _uriService { get; }
        public ILocalFileHandler _fileHandler { get; }

        public bool DeleteFileLogs(string folderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            bool result = false;
            var files = dir.GetFiles();

            if (files.Count() <= 0) return true;

            foreach (var file in files)
            {
                result = _fileHandler.Delete(file);
            }
            return result;
        }

        public IQueryable<EventLog> Get() => _repository.Get();

        public IQueryable<EventLog> Get(Expression<Func<EventLog, bool>> filter) => _repository.Get(filter);

        public EventLog Get(int id) => _repository.Get(id);

        public PaginatedList<EventLog> GetPaginatedList(string route, int pageIndex, int pageSize) =>
            new PaginatedList<EventLog>(_repository.Get(), _uriService, route, pageIndex, pageSize);
    }
}
