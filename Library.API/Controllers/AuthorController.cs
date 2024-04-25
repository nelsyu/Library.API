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

        [HttpGet("{authorId}", Name = nameof(GetAuthor))]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var author = AuthorRepository.GetAuthor(authorId);

            if (author == null)
            {
                return NotFound();
            }
            else
            {
                return author;
            }
        }

        [HttpPost]
        public IActionResult CreateAuthor(AuthorForCreationDto authorForCreationDto)
        {
            var authorDto = new AuthorDto
            {
                Name = authorForCreationDto.Name,
                Age = authorForCreationDto.Age,
                Email = authorForCreationDto.Email
            };

            AuthorRepository.AddAuthor(authorDto);
            return CreatedAtRoute(nameof(GetAuthor), new { authorId = authorDto.Id }, authorDto);
        }

        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(Guid authorId)
        {
            var author = AuthorRepository.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }

            AuthorRepository.DeleteAuthor(author);
            return NoContent();
        }
    }
}
