using AutoMapper;
using FluentValidation.Results;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
using NETCore.Basic.Services.Data;
using NETCore.Basic.Services.External;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Tests.Services.External;
using NETCore.Basic.Util.Crypto;
using NETCore.Basic.Util.Extentions;
using NETCore.Basic.Util.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace NETCore.Basic.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administrator")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserServices _userService { get; }
        private IMapper _mapper { get; }
        private IAzureStorage _azureStorage { get; }
        private ILocalFileHandler _fileHandler { get; }
        private IUriService _uriService { get; }
        private IAuthService _authService { get; }
        private IUrlHelper _urlHelper { get; }

        public UsersController(IUserServices userService, IMapper mapper, IAzureStorage azureStorage, ILocalFileHandler fileHandler, IUriService uriService, IAuthService authService, IUrlHelper urlHelper)
        {
            _userService = userService;
            _mapper = mapper;
            _azureStorage = azureStorage;
            _fileHandler = fileHandler;
            _uriService = uriService;
            _authService = authService;
            _urlHelper = urlHelper;
        }
        /// <summary>
        /// Paginated response for user list along with HATEOAS
        /// </summary>
        /// <param name="query">Receives pageIndex and pageSize</param>
        /// <returns>Paged object with list of Users</returns>
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(PaginatedList<OutputUser>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetAll([FromQuery] PaginationFilter query)
        {
            try
            {
                var route = Request.Path.Value;
                var paginatedList = _userService.GetPaginatedList(_uriService, route, query.PageIndex, query.PageSize);

                if (paginatedList.TotalCount <= 0)
                    return NotFound();

                return Ok(paginatedList);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        [Route("getH")]
        [ProducesResponseType(typeof(HATEOASResult<OutputUser>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Geth([ModelBinder(typeof(CustomUserIdBinder))] string[] ids)
        {
            try
            {
                if (ids?.Length <= 0)
                    return NoContent();
                List<HATEOASResult<User>> result = new List<HATEOASResult<User>>(ids.Length);

                foreach (var id in ids)
                {
                    var newId = Convert.ToInt32(id);
                    var hateoasResult = _userService.GetHateoas(newId);
                    AddLinksHATEOAS(hateoasResult, newId);
                    result.Add(hateoasResult);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        [HttpGet]
        [Route("get/{Id}")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get(int Id)
        {
            var userCtx = _userService.Get(Id);
            if (userCtx == null)
                return NoContent();
            var mappedUser = _mapper.Map<OutputUser>(userCtx);

            return Ok(mappedUser);
        }

        [HttpPost]
        [Route("post")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Post([FromBody] InputUser user)
        {
            var mappedUser = _mapper.Map<User>(user);

            var success = _userService.Add(mappedUser, out List<ValidationFailure> errors);
            if (success)
                return CreatedAtAction(nameof(Post), success);
            else
                return StatusCode(500, errors.Select(err => err.ErrorMessage));
        }

        [HttpPut]
        [Route("put/{id}")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Put([FromBody] InputUser user)
        {
            var mappedUser = _mapper.Map<User>(user);

            var success = _userService.Update(mappedUser, out List<ValidationFailure> errors);
            if (success)
                return CreatedAtAction(nameof(Post), success);
            else
                return StatusCode(500, errors.Select(err => err.ErrorMessage));
        }
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Authenticate([FromBody] InputUser user)
        {
            var mappedUser = _mapper.Map<InputUser, User>(user);
            var result = _userService.Authenticate(mappedUser, out User authenticatedUser);
            var response = new AuthenticationResponse() { IsAuthenticated = result };

            if (result)
                response.Token = _authService.GenerateToken(authenticatedUser);

            return Ok(response);
        }

        private void AddLinksHATEOAS<T>(HATEOASResult<T> hateoas, int id) where T : class
        {
            hateoas.AddLink("self", _uriService.GetUri($"/api/users/{Method.GET}/{id}"), Method.GET);
            hateoas.AddLink("update-user", _uriService.GetUri($"/api/users/{nameof(Put)}/{id}"), Method.PUT);
            hateoas.AddLink("delete-user", _uriService.GetUri($"/api/users/{nameof(Delete)}/{id} "), Method.DELETE);
        }
    }
}
