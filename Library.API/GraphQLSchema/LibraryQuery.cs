using GraphQL.Types;
using Library.API.Services;

namespace Library.API.GraphQLSchema
{
    public class LibraryQuery : ObjectGraphType
    {
        public LibraryQuery(IRepositoryWrapper repositoryWrapper)
        {
            Field<ListGraphType<AuthorType>>("authors").Resolve(context =>
                repositoryWrapper.Author.GetAllAsync().Result);

            Field<AuthorType>("author").Arguments(new QueryArguments(new QueryArgument<IdGraphType>()
            {
                Name = "id",
            }))
                .Resolve(context =>
                {
                    Guid id = Guid.Empty;
                    if (context.Arguments!.TryGetValue("id", out GraphQL.Execution.ArgumentValue value))
                    {
                        id = new Guid(value.Value!.ToString()!);
                    }

                    return repositoryWrapper.Author.GetByIdAsync(id).Result;
                });
        }
    }
}
