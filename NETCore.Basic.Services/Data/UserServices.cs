using AutoMapper;
using FluentValidation.Results;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Services.Validation;
using NETCore.Basic.Util.Crypto;
using NETCore.Basic.Util.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NETCore.Basic.Services.Data
{
    public interface IUserServices
    {
        bool Add(User user, out List<ValidationFailure> errors);
        bool Authenticate(User user, out User ctxUser);
        IQueryable<User> Get();
        IQueryable<User> Get(Expression<Func<User, bool>> filter);
        User Get(int id);
        HATEOASResult<User> GetHateoas(int id);
        bool Update(User user, out List<ValidationFailure> errors);
        void Delete(int id);

        PaginatedList<OutputUser> GetPaginatedList(string route, int pageIndex, int pageSize);

    }
    public class UserServices : IUserServices
    {
        public IValidator<User> _validationRules { get; }
        public IRepository<User> _repository { get; }
        public IHashing _hashingService { get; }
        public IMapper _mapper { get; }
        public IUriService _uriService { get; }

        public UserServices(IValidator<User> validationRules, IRepository<User> repository, IHashing hashingService, IMapper mapper, IUriService uriService)
        {
            _validationRules = validationRules;
            _repository = repository;
            _hashingService = hashingService;
            _mapper = mapper;
            _uriService = uriService;
        }


        public bool Add(User user, out List<ValidationFailure> errors)
        {
            var validationResult = _validationRules.Validate(user);
            errors = validationResult.Errors;

            if (!validationResult.IsValid && validationResult.Errors.Any()) return false;

            user.Password = _hashingService.ComputeHash(user.Password);
            user.RegistredAt = DateTime.Now;

            var userCtx = _repository.Add(user);
            _repository.SaveChanges();

            return userCtx.Id >= 1;
        }
        public bool Authenticate(User user, out User dbUser)
        {
            dbUser = _repository.Get(u => u.Username == user.Username).SingleOrDefault();
            if (dbUser is null) return false;

            return _hashingService.VerifyHash(user.Password, dbUser.Password);
        }
        public IQueryable<User> Get() => _repository.Get();

        public User Get(int id) => _repository.Get(id);
        public IQueryable<User> Get(Expression<Func<User, bool>> filter) => _repository.Get(filter);

        public void Delete(int id)
        {
            var ctxUser = Get(id);
            if (ctxUser is null) throw new Exception("User not found");

            _repository.Delete(ctxUser);
            _repository.SaveChanges();
        }

        public bool Update(User user, out List<ValidationFailure> errors)
        {
            var validationResult = _validationRules.Validate(user);
            errors = validationResult.Errors;

            if (!validationResult.IsValid && validationResult.Errors.Any()) return false;

            user.Password = _hashingService.ComputeHash(user.Password);
            var ctxUsr = _repository.Update(user);
            _repository.SaveChanges();
            return !user.Equals(ctxUsr);
        }

        public PaginatedList<OutputUser> GetPaginatedList(string route, int pageIndex, int pageSize)
        {
            var mappedList = _mapper.ProjectTo<OutputUser>(_repository.Get(), typeof(OutputUser));

            return new PaginatedList<OutputUser>(mappedList, _uriService, route, pageIndex, pageSize);
        }

        public HATEOASResult<User> GetHateoas(int id)
        {
            var user = Get(id);
            if (user is null) return null;

            var hateoas = new HATEOASResult<User>(user);
            AddLinksHATEOAS(hateoas, id);
            return hateoas;
        }
        private void AddLinksHATEOAS<T>(HATEOASResult<T> hateoas, int id) where T : class
        {
            hateoas.AddLink("get-user", _uriService.GetUri($"/api/users/get/{id}"), Method.GET);
            hateoas.AddLink("update-user", _uriService.GetUri($"/api/users/get/{id}"), Method.PUT);
            hateoas.AddLink("delete-user", _uriService.GetUri($"/api/users/delete/{id} "), Method.DELETE);
        }
    }
}
