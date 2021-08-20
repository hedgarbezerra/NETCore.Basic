using AutoMapper;
using FluentValidation.Results;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
using NETCore.Basic.Services.DataServices;
using NETCore.Basic.Services.Pagination;
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
        public IHashing _hash { get; }
        public ILocalFileHandler _fileHandler { get; }
        public IFileHandler<HtmlDocument> _htmlHandler { get; }
        public IEncryption _encryption { get; }

        public UsersController(IUserServices userService, IMapper mapper, IUriService uriService, IEncryption encryption)
        {
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
            _encryption = encryption;
        }

        [Route("crypto")]
        [ProducesResponseType(typeof(PaginatedList<User>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Crypto()
        {
            var msg = "oi sou hedgar";
            var encryptedMsg = _encryption.Encrypt(msg);
            return Ok(encryptedMsg);
        }
        [HttpGet]
        [Route("decrypt")]
        [ProducesResponseType(typeof(PaginatedList<User>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Decrypt()
        {
            var msg = "oi sou hedgar";
            var encryptedMsg = _encryption.Encrypt(msg);
            var decryptedMsg = _encryption.Decrypt(encryptedMsg);
            return Ok(decryptedMsg);
        }

        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(typeof(PaginatedList<User>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get([FromQuery] PaginationFilter query)
        {
            Log.Information("##Starting Log");

            var route = Request.Path.Value;
            var paginatedList = _userService.GetPaginatedList(_uriService, route, query.PageIndex, query.PageSize);

            if (paginatedList.TotalCount <= 0)
                return NotFound();

            Log.Information("##Finishing Log");
            return Ok(paginatedList);
        }

        [HttpGet]
        [Route("filter/{Id}")]
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
        [Route("filter/{userId}/mail")]
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

        [HttpGet]
        [Route("delete")]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult DeleteFolder([FromQuery]string path)
        {
            try
            {
                _fileHandler.DeleteFolder(path);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
