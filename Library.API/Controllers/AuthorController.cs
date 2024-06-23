using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }
        public ILogger<AuthorController> Logger { get; }
        public IDistributedCache DistributedCache { get; }

        public AuthorController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<AuthorController> logger, IDistributedCache distributedCache)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            Logger = logger;
            DistributedCache = distributedCache;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetAuthorsAsync))]
        public async Task<ActionResult<ResourceCollection<AuthorDto>>> GetAuthorsAsync([FromQuery]AuthorResourceParameters parameters)
        {
            PagedList<Author>? pagedList = null;

            // 為了簡單，僅當請求中不包含過濾和搜尋查詢字串時，
            // 才進行緩存，實際情況不應有此限制
            if (string.IsNullOrWhiteSpace(parameters.BirthPlace) && string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                string cacheKey = $"authors_page_{parameters.PageNumber}_pageSize_{parameters.PageSize}_{parameters.SortBy}";
                string? cachedContent = await DistributedCache.GetStringAsync(cacheKey);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new PagedListConverter<Author>());
                settings.Formatting = Formatting.Indented;

                if (string.IsNullOrWhiteSpace(cachedContent))
                {
                    pagedList = await RepositoryWrapper.Author.GetAllAsync(parameters);
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };

                    var serializedContent = JsonConvert.SerializeObject(pagedList, settings);
                    await DistributedCache.SetStringAsync(cacheKey, serializedContent, options);
                }
                else
                {
                    pagedList = JsonConvert.DeserializeObject<PagedList<Author>>(cachedContent, settings);
                }
            }
            else
            {
                pagedList = await RepositoryWrapper.Author.GetAllAsync(parameters);
            }

            var paginationMetadata = new
            {
                totalCount = pagedList!.TotalCount,
                pageSize = pagedList.PageSize,
                currentPage = pagedList.CurrentPage,
                totalPages = pagedList.TotalPages,
                previousPageLink = pagedList.HasPrevious ? Url.Link(nameof(GetAuthorsAsync), new
                {
                    pageNumber = pagedList.CurrentPage - 1,
                    pageSize = pagedList.PageSize,
                    birthPlace = parameters.BirthPlace,
                    searchQuery = parameters.SearchQuery,
                    sortBy = parameters.SortBy
                }) : null,
                nextPageLink = pagedList.HasNext ? Url.Link(nameof(GetAuthorsAsync), new
                {
                    pageNumber = pagedList.CurrentPage + 1,
                    pageSize = pagedList.PageSize,
                    birthPlace = parameters.BirthPlace,
                    searchQuery = parameters.SearchQuery,
                    sortBy = parameters.SortBy
                }) : null
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(pagedList);
            authorDtoList = authorDtoList.Select(author => CreateLinksForAuthor(author));

            var resourceList = new ResourceCollection<AuthorDto>(authorDtoList.ToList());
            return CreateLinksForAuthors(resourceList, parameters, paginationMetadata);
        }

        [HttpGet("{authorId}", Name = nameof(GetAuthorAsync))]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<AuthorDto>> GetAuthorAsync(Guid authorId)
        {
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }

            var entityHash = HashFactory.GetHash(author);
            Response.Headers[HeaderNames.ETag] = entityHash;
            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var requestETag) && entityHash == requestETag)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            var authorDto = Mapper.Map<AuthorDto>(author);
            return CreateLinksForAuthor(authorDto);
        }

        [HttpPost(Name = nameof(CreateAuthorAsync))]
        public async Task<ActionResult> CreateAuthorAsync(AuthorForCreationDto authorForCreationDto)
        {
            var author = Mapper.Map<Author>(authorForCreationDto);

            RepositoryWrapper.Author.Create(author);
            var result = await RepositoryWrapper.Author.SaveAsync();
            if (!result)
            {
                throw new Exception("建立資源author失敗");
            }

            var authorCreated = Mapper.Map<AuthorDto>(author);
            return CreatedAtRoute(nameof(GetAuthorAsync), new { authorId = authorCreated.Id }, CreateLinksForAuthor(authorCreated));
        }

        [HttpDelete("{authorId}", Name = nameof(DeleteAuthorAsync))]
        public async Task<ActionResult> DeleteAuthorAsync(Guid authorId)
        {
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }

            RepositoryWrapper.Author.Delete(author);
            var result = await RepositoryWrapper.Author.SaveAsync();
            if (!result)
            {
                throw new Exception("刪除資源author失敗");
            }

            return NoContent();
        }

        private AuthorDto CreateLinksForAuthor(AuthorDto author)
        {
            author.Links.Clear();
            author.Links.Add(new Link(HttpMethods.Get, "self", Url.Link(nameof(GetAuthorAsync), new { authorId = author.Id })));
            author.Links.Add(new Link(HttpMethods.Delete, "delete author", Url.Link(nameof(DeleteAuthorAsync), new { authorId = author.Id })));
            author.Links.Add(new Link(HttpMethods.Get, "author's books", Url.Link(nameof(BookController.GetBooksAsync), new { authorId = author.Id })));

            return author;
        }

        private ResourceCollection<AuthorDto> CreateLinksForAuthors(ResourceCollection<AuthorDto> authors, AuthorResourceParameters? parameters = null, dynamic? paginationData = null)
        {
            authors.Links.Clear();
            authors.Links.Add(new Link(HttpMethods.Get, "self", Url.Link(nameof(GetAuthorsAsync), parameters)));

            authors.Links.Add(new Link(HttpMethods.Post, "create author", Url.Link(nameof(CreateAuthorAsync), null)));

            if (paginationData != null)
            {
                if (paginationData.previousPageLink != null)
                {
                    authors.Links.Add(new Link(HttpMethods.Get, "previous page", paginationData.previousPageLink));
                }

                if (paginationData.nextPageLink != null)
                {
                    authors.Links.Add(new Link(HttpMethods.Get, "next page", paginationData.nextPageLink));
                }
            }

            return authors;
        }
    }
}
