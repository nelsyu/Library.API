using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest
{
    [ApiController]
    [Route("api/news")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class NewsController : ControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public ActionResult<string> Get() => "Result from v1";

        [HttpGet, MapToApiVersion("2.0")]
        [ApiExplorerSettings(GroupName = "v2")]
        public ActionResult<string> GetV2() => "Result from v2";
    }
}
