using FluentValidation.Results;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NETCore.Basic.Services.DataServices
{
    public interface IUserServices
    {
        bool Add(User user, out List<ValidationFailure> errors);
        IQueryable<User> Get();
        IQueryable<User> Get(Expression<Func<User, bool>> filter);
        User Get(int id);
        PaginatedList<User> GetPaginatedList(IUriService uriService, string route, int pageIndex, int pageSize);

    }
    public class UserServices : IUserServices
    {
        public IValidator<User> _validationRules { get; }
        public IRepository<User> _repository { get; }
        public UserServices(IValidator<User> validationRules, IRepository<User> repository)
        {
            _validationRules = validationRules;
            _repository = repository;
        }


        public bool Add(User user, out List<ValidationFailure> errors)
        {
            var validationResult = _validationRules.Validate(user);
            errors = validationResult.Errors;

            if (!validationResult.IsValid && validationResult.Errors.Any()) return false;

            var userCtx = _repository.Add(user);
            _repository.SaveChanges();
            return userCtx.Id >= 1;
        }

        public IQueryable<User> Get()
        {
            return _repository.Get();
        }

        public User Get(int id)
        {
            return _repository.Get(id);
        }

        public IQueryable<User> Get(Expression<Func<User, bool>> filter)
        {
            return _repository.Get(filter);
        }

        public PaginatedList<User> GetPaginatedList(IUriService uriService, string route, int pageIndex, int pageSize)
        {
            return new PaginatedList<User>(_repository.Get(), uriService, route, pageIndex, pageSize);
        }
    }
}
