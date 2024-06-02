using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controller.VersionTest.V2
{
    [ApiController]
    [Route("api/person")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Result from v2";
    }
}
