using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest.V2
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public string Get() => "Result from v2";
    }
}
