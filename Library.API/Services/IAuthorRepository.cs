using Library.API.Entities;

namespace Library.API.Services
{
    public interface IAuthorRepository : IRepositoryBase<Author>, IRepositoryBase2<Author, Guid>
    {
    }
}
