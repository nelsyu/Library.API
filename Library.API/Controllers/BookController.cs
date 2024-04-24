﻿using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public IAuthorRepository AuthorRepository { get; }
        public IBookRepository BookRepository { get; }

        public BookController(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            AuthorRepository = authorRepository;
            BookRepository = bookRepository;
        }

        [HttpGet]
        public ActionResult<List<BookDto>> GetBooks(Guid authorId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            return BookRepository.GetBooksForAuthor(authorId).ToList();
        }

        [HttpGet("{bookId}", Name = nameof(GetBook))]
        public ActionResult<BookDto> GetBook(Guid authorId, Guid bookId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var targetBook = BookRepository.GetBookForAuthor(authorId, bookId);
            if (targetBook == null)
            {
                return NotFound();
            }

            return targetBook;
        }

        [HttpPost]
        public IActionResult AddBook(Guid authorId, BookForCreationDto bookForCreationDto)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var newBook = new BookDto
            {
                Id = Guid.NewGuid(),
                Title = bookForCreationDto.Title,
                Description = bookForCreationDto.Description,
                Pages = bookForCreationDto.Pages,
                AuthorId = authorId
            };

            BookRepository.AddBook(newBook);
            return CreatedAtRoute(nameof(GetBook), new { authorId = authorId, bookId = newBook.Id }, newBook);
        }
    }
}