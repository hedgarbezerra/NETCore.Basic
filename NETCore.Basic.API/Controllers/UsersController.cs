using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
using NETCore.Basic.Services.DataServices;
using NETCore.Basic.Services.Pagination;
using System;
using System.Collections.Generic;
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

        public UsersController(IRepository<User> repository, IMapper mapper, IUriService uriService)
        {
            _repository = repository;
            _mapper = mapper;
            _uriService = uriService;
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
    }
}
