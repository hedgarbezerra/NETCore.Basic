using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Domain.Models.Users;
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

        public UsersController(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get([FromQuery] PaginationFilter query)
        {
            return Ok(new PaginatedList<User>(_repository.Get(), query.PageIndex, query.PageSize));
        }

        [HttpGet]
        [Route("filter/{Id}")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetById([FromQuery]int Id)
        {
            return Ok(_repository.Get(Id));
        }

        [HttpGet]
        [Route("filter/{userId}/mail")]
        [ProducesResponseType(typeof(OutputUser), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult Get([FromQuery] int userId)
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

            return Ok(_repository.Add(mappedUser));
        }
    }
}
