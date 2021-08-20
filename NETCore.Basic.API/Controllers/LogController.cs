using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Services.Data;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Util.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Basic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        public IUriService _uriService { get; }
        public ILoggingService _loggingService { get; }
        public IHostingEnvironment _env { get; }

        public LogController(IUriService uriService, ILoggingService loggingService, IHostingEnvironment env)
        {
            _uriService = uriService;
            _loggingService = loggingService;
            _env = env;
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(PaginatedList<EventLog>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult GetLog([FromQuery] PaginationFilter query)
        {
            var route = Request.Path.Value;
            var paginatedList = _loggingService.GetPaginatedList(_uriService, route, query.PageIndex, query.PageSize);

            if (paginatedList.TotalCount <= 0)
                return NotFound();

            return Ok(paginatedList);
        }


        [HttpDelete]
        [Route("cleanfilelog")]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public IActionResult DeleteFolder()
        {
            try
            {
                var pathRoot = _env.WebRootPath;
                var result = _loggingService.DeleteFileLogs(pathRoot + "\\logs");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
