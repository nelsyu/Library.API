using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.VersionTest.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public string Get() => "Result from v1";
    }
}
