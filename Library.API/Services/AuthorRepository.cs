using Library.API.Entities;
using Library.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public class AuthorRepository : RepositoryBase<Author, Guid>, IAuthorRepository
    {
        public AuthorRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<PagedList<Author>> GetAllAsync(AuthorResourceParameters parameters)
        {
            IQueryable<Author> queryableAuthors = DbContext.Set<Author>();
            if (!string.IsNullOrWhiteSpace(parameters.BirthPlace))
            {
                queryableAuthors = queryableAuthors.Where(m => m.BirthPlace!.ToLower() == parameters.BirthPlace);
            }

            return PagedList<Author>.CreateAsync(queryableAuthors, parameters.PageNumber, parameters.PageSize);
        }
    }
}
