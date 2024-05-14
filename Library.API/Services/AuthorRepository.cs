using Library.API.Entities;
using Library.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                queryableAuthors = queryableAuthors.Where(
                    m => m.BirthPlace!.ToLower().Contains(parameters.SearchQuery.ToLower())
                    || m.Name!.ToLower().Contains(parameters.SearchQuery.ToLower()));
            }

            var orderedAuthors = queryableAuthors.OrderBy(parameters.SortBy);
            return PagedList<Author>.CreateAsync(orderedAuthors, parameters.PageNumber, parameters.PageSize);
        }
    }
}
