using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NETCore.Basic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;

        public EmployeesController(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.Get()
                .ToList();

            if (!result.Any())
                return NoContent();

            return Ok(result);
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_repository.Get(id));


        [HttpPost]
        public IActionResult Post([FromBody] Employee empregado)
        {
            var result = _repository.Add(empregado);
            _repository.SaveChanges();

            return Ok(result.Id > 0);
        }
    }
}
