using Library.API.Models;

namespace Library.API.Services
{
    public interface IAuthorRepository
    {
        IEnumerable<AuthorDto> GetAuthors();
        AuthorDto? GetAuthor(Guid authorId);
        bool IsAuthorExists(Guid authorId);
        void AddAuthor(AuthorDto author);
        void DeleteAuthor(AuthorDto author);
    }
}
