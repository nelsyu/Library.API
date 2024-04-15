using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        public AuthorController(IAuthorRepository authorRepository)
        {
            AuthorRepository = authorRepository;
        }

        public IAuthorRepository AuthorRepository { get; }

        [HttpGet()]
        public ActionResult<List<AuthorDto>> GetAuthors()
        {
            return AuthorRepository.GetAuthors().ToList();
        }
    }
}
