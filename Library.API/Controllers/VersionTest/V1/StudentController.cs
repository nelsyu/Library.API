using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/students")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Result from v1";
    }
}
