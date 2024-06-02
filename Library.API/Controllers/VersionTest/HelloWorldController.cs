using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/[controller]")]
    [NonController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public string Get() => "Hello world!";

        [HttpGet, MapToApiVersion("2.0")]
        [ApiExplorerSettings(GroupName = "v2")]
        public string GetV2() => "Hello world v2.0!";
    }
}
