using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/students")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Result from v2";
    }
}
