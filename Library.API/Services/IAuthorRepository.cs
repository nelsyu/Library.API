using Library.API.Entities;
using Library.API.Helpers;

namespace Library.API.Services
{
    public interface IAuthorRepository : IRepositoryBase<Author>, IRepositoryBase2<Author, Guid>
    {
        Task<PagedList<Author>> GetAllAsync(AuthorResourceParameters parameters);
    }
}
