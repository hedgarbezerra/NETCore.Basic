using AutoMapper;
using FluentValidation.Results;
using HtmlAgilityPack;
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
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUserServices _userService { get; }
        public IMapper _mapper { get; }
        public IUriService _uriService { get; }
        public IEncryption _encryption { get; }
        public IMailing _mailHandler { get; }
        public IAzureStorage _azureStorage { get; }

        public UsersController(IUserServices userService, IMapper mapper, IUriService uriService, IEncryption encryption, IMailing mailHandler, IAzureStorage azureStorage)
        {
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
            _encryption = encryption;
            _mailHandler = mailHandler;
            _azureStorage = azureStorage;
        }
        [HttpGet]
        [Route("storage")]
        public IActionResult AzureStorage()
        {
            var storageItems = _azureStorage.GetAllAsync();
            var storaSingle = _azureStorage.Get("Boleto_Lello_32854859.pdf");
            return Ok();
        }

        [HttpGet]
        [Route("mail")]
        public IActionResult Mail(string email)
        {
            _mailHandler.Notify(email, "Teste", "Este é um teste ok");

            return Ok();
        }

        /// <summary>
        /// Paginated response for user list along with HATEOAS
        /// </summary>
        /// <param name="query">Receives pageIndex and pageSize</param>
        /// <returns>Paged object with list of Users</returns>
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(PaginatedList<OutputUser>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get([FromQuery] PaginationFilter query)
        {
            var route = Request.Path.Value;
            var paginatedList = _userService.GetPaginatedList(_uriService, route, query.PageIndex, query.PageSize);

            if (paginatedList.TotalCount <= 0)
                return NotFound();

            return Ok(paginatedList);
        }

        [HttpGet]
        [Route("get/{Id}")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetById(int Id)
        {
            var userCtx = _userService.Get(Id);
            if (userCtx == null)
                return NoContent();
            var mappedUser = _mapper.Map<OutputUser>(userCtx);
            return Ok(mappedUser);
        }

        [HttpGet]
        [Route("get/{userId}/mail")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get(int userId)
        {
            return Ok(_userService.Get(userId).Email);
        }

        [HttpPost]
        [Route("insert")]
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

    }
}
