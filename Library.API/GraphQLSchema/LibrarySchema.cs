using GraphQL.Types;

namespace Library.API.GraphQLSchema
{
    public class LibrarySchema : Schema
    {
        public LibrarySchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<LibraryQuery>();
        }
    }
}
