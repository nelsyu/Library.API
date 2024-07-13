using Library.API.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private List<Student> students = new List<Student>();
        public IDataProtectionProvider DataProtectionProvider { get; set; }

        public ValuesController(IDataProtectionProvider dataProtectionProvider)
        {
            DataProtectionProvider = dataProtectionProvider;
            students.Add(new Student
            {
                Id = "1",
                Name = "Jim"
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            var protector = DataProtectionProvider.CreateProtector("ProtectResourceId");
            var result = students.Select(s => new Student
            {
                Id = protector.Protect(s.Id!),
                Name = s.Name
            });

            return result.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Student> Get(string id)
        {
            var protector = DataProtectionProvider.CreateProtector("ProtectResourceId");
            var rawId = protector.Unprotect(id);
            var targetItem = students.FirstOrDefault(s => s.Id == rawId);
            return new Student { Id = id, Name = targetItem!.Name };
        }
    }
}
