using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Basic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DIExampleController : ControllerBase
    {
        private IMapping _mapping;
        public DIExampleController(Func<EMappingType, IMapping> serviceResolver)
        {
            _mapping = serviceResolver(EMappingType.Example);
        }
    }

    [Route("api/[controller]2")]
    [ApiController]
    public class DIExample2Controller : ControllerBase
    {
        private IMapping _mapping;
        public DIExample2Controller(IEnumerable<IMapping> mappingServices)
        {
            _mapping = mappingServices.Where(x =>
            {
                var servic = x as ExampleMapping;
                return servic != null;
            }).SingleOrDefault();
        }
    }
}
