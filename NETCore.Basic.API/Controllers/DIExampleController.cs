using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Basic.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Basic.API.Controllers
{
    [Route("api/exempleclass1")]
    [ApiController]
    public class DIExampleController : ControllerBase
    {
        private IMapping _mapping;
        public DIExampleController(Func<EMappingType, IMapping> serviceResolver)
        {
            _mapping = serviceResolver(EMappingType.Example);
        }
    }

    [Route("api/exempleclass2")]
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

    [Route("api/exempleclass3")]
    [ApiController]
    public class DIExample3Controller : ControllerBase
    {
        private IMapping _mapping;
        public DIExample3Controller(IServiceProvider serviceProvider)
        {
            _mapping = (ExampleMapping)serviceProvider.GetService (typeof(IMapping));
        }

    }
}
