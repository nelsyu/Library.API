using Library.API.Entities;

namespace Library.API.Services
{
    public interface IBookRepository : IRepositoryBase<Book>, IRepositoryBase2<Book, Guid>
    {
    }
}
