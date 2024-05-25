using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    [ServiceFilter(typeof(CheckAuthorExistFilterAttribute))]
    public class BookController : ControllerBase
    {
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }
        public IMemoryCache MemoryCache { get; }

        public BookController(IRepositoryWrapper repositoryWrapper, IMapper mapper, IMemoryCache memoryCache)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            MemoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>?>> GetBooksAsync(Guid authorId)
        {
            List<BookDto>? bookDtoList = new List<BookDto>();
            string key = $"{authorId}_books";
            if (!MemoryCache.TryGetValue(key, out bookDtoList))
            {
                var books = await RepositoryWrapper.Book.GetBooksAsync(authorId);
                bookDtoList = Mapper.Map<IEnumerable<BookDto>>(books).ToList();
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
                options.Priority = CacheItemPriority.Normal;
                MemoryCache.Set(key, bookDtoList, options);
            }

            return bookDtoList;
        }

        [HttpGet("{bookId}", Name = nameof(GetBookAsync))]
        public async Task<ActionResult<BookDto>> GetBookAsync(Guid authorId, Guid bookId)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }

            var bookDto = Mapper.Map<BookDto>(book);

            return bookDto;
        }

        [HttpPost]
        public async Task<IActionResult> AddBookAsync(Guid authorId, BookForCreationDto bookForCreationDto)
        {
            var book = Mapper.Map<Book>(bookForCreationDto);

            book.AuthorId = authorId;
            RepositoryWrapper.Book.Create(book);
            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("建立資源Book失敗");
            }

            var bookDto = Mapper.Map<BookDto>(book);
            return CreatedAtRoute(nameof(GetBookAsync), new { authorId = bookDto.AuthorId, bookId = bookDto.Id }, bookDto);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookAsync(Guid authorId, Guid bookId)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }

            RepositoryWrapper.Book.Delete(book);
            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("刪除資源Book失敗");
            }
            return NoContent();
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBookAsync(Guid authorId, Guid bookId, BookForUpdateDto updatedBook)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }

            Mapper.Map(updatedBook, book, typeof(BookForUpdateDto), typeof(Book));
            RepositoryWrapper.Book.Update(book);
            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("更新資源Book失敗");
            }
            return NoContent();
        }

        [HttpPatch("{bookId}")]
        public async Task<IActionResult> ParticallyUpdateBookAsync(Guid authorId, Guid bookId, JsonPatchDocument<BookForUpdateDto> patchDocument)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }

            var bookUpdateDto = Mapper.Map<BookForUpdateDto>(book);
            patchDocument.ApplyTo(bookUpdateDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(bookUpdateDto, book, typeof(BookForUpdateDto), typeof(Book));

            RepositoryWrapper.Book.Update(book);
            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("更新資源Book失敗");
            }
            return NoContent();
        }
    }
}
