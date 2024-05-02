using Library.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public class BookRepository : RepositoryBase<Book, Guid>, IBookRepository
    {
        public BookRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<Book>> GetBooksAsync(Guid authorId)
        {
            return Task.FromResult(DbContext.Set<Book>().Where(book => book.AuthorId == authorId).AsEnumerable());
        }

        public async Task<Book?> GetBookAsync(Guid authorId, Guid bookId)
        {
            return await DbContext.Set<Book>().SingleOrDefaultAsync(book => book.AuthorId == authorId && book.Id == bookId);
        }
    }
}
