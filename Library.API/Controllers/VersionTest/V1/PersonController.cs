using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest.V1
{
    [ApiController]
    [Route("api/person")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Result from v1";
    }
}
