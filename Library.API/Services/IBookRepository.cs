using Library.API.Models;

namespace Library.API.Services
{
    public interface IBookRepository
    {
        IEnumerable<BookDto> GetBooksForAuthor(Guid authorId);
        BookDto? GetBookForAuthor(Guid authorId, Guid bookId);
    }
}
