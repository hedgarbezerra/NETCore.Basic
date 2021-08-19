using AutoMapper;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
using NETCore.Basic.Services.DataServices;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Util.Helper;
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
        public IRepository<User> _repository { get; }
        public IMapper _mapper { get; }
        public IUriService _uriService { get; }
        public ILocalFileHandler _fileHandler { get; }
        public IHTMLHandler _htmlHandler { get; }

        public UsersController(IRepository<User> repository, IMapper mapper, IUriService uriService, ILocalFileHandler fileHandler, IHTMLHandler htmlHandler)
        {
            _repository = repository;
            _mapper = mapper;
            _uriService = uriService;
            _fileHandler = fileHandler;
            _htmlHandler = htmlHandler;
        }

        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(typeof(PaginatedList<User>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get([FromQuery] PaginationFilter query)
        {
            var result = _repository.Get();
            var route = Request.Path.Value;
            var paginatedList = new PaginatedList<User>(result, _uriService,route, query.PageIndex, query.PageSize);

            if (paginatedList.TotalCount <= 0)
                return NotFound();

            return Ok(paginatedList);
        }

        [HttpGet]
        [Route("filter/{Id}")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetById(int Id)
        {
            var userCtx = _repository.Get(Id);
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
            return Ok(_repository.Get(userId).Email);
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Post([FromBody] InputUser user)
        {
            var mappedUser = _mapper.Map<User>(user);

            var userCtx = _repository.Add(mappedUser);
            _repository.SaveChanges();
            return CreatedAtAction(nameof(UsersController), userCtx);
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
        [HttpGet]
        [Route("readfile")]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult ReadFile([FromQuery] string path)
        {
            try
            {
                var arquivo = _fileHandler.Read(path, out Stream fileStream);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet]
        [Route("writefile")]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult WriteFile([FromQuery] string from, string to)
        {
            try
            {
                var arquivo = _fileHandler.Read(from, out Stream fileStream);
                _fileHandler.Write(fileStream,to, "teste_2.html");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("readhtml")]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult ReadHtml([FromQuery] string path)
        {
            try
            {
                var sucesso = _htmlHandler.Read(path, out HtmlDocument htmlDocument);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet]
        [Route("writehtml")]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult WriteHtml([FromQuery] string path)
        {
            try
            {
                var fakeHtml = @"<!DOCTYPE html>
                                    <html lang='pt-br'>
                                    <head>
                                        <meta charset='UTF-8'>
                                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                        <title>DOC</title>
                                    </head>
                                    <body>
                                        <p>Teste</p>
                                        <div class='ok'>
                                            <p>OK</p>
                                        </div>
                                    </body>
                                    </html>";
                var html = new HtmlDocument();
                html.LoadHtml(fakeHtml);
                var sucesso = _htmlHandler.Write(html, path, "teste_2.html");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
