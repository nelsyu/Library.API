﻿using Library.API.Entities;
using Library.API.Extensions;
using Library.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Library.API.Services
{
    public class AuthorRepository : RepositoryBase<Author, Guid>, IAuthorRepository
    {
        private Dictionary<string, PropertyMapping>? mappingDict = null;

        public AuthorRepository(DbContext dbContext) : base(dbContext)
        {
            mappingDict = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);
            mappingDict.Add("Name", new PropertyMapping("Name"));
            mappingDict.Add("Age", new PropertyMapping("BirthDate", true));
            mappingDict.Add("BirthPlace", new PropertyMapping("BirthPlace"));
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

            var orderedAuthors = queryableAuthors.Sort(parameters.SortBy, mappingDict!);
            return PagedList<Author>.CreateAsync(orderedAuthors, parameters.PageNumber, parameters.PageSize);
        }
    }
}
