using Library.API.Data;
using Library.API.Models;

namespace Library.API.Services
{
    public class AuthorMockRepository : IAuthorRepository
    {
        public AuthorDto? GetAuthor(Guid authorId)
        {
            var author = LibraryMockData.Current.Authors.FirstOrDefault(au => au.Id == authorId);
            return author;
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            return LibraryMockData.Current.Authors;
        }

        public bool IsAuthorExists(Guid authorId)
        {
            return LibraryMockData.Current.Authors.Any(au => au.Id == authorId);
        }
    }
}
