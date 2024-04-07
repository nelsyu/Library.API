﻿using Library.API.Data;
using Library.API.Models;

namespace Library.API.Services
{
    public class BookMockRepository : IBookRepository
    {
        public BookDto? GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return LibraryMockData.Current.Books.FirstOrDefault(b => b.AuthorId == authorId && b.Id == bookId);
        }

        public IEnumerable<BookDto> GetBooksForAuthor(Guid authorId)
        {
            return LibraryMockData.Current.Books.Where(b => b.AuthorId == authorId).ToList();
        }
    }
}
