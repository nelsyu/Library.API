using GraphQL;
using GraphQL.Types;
using Library.API.GraphQLSchema;

namespace Library.API.Extensions
{
    public static class GraphQLExtensions
    {
        public static void AddGraphQLSchemaAndTypes(this IServiceCollection services)
        {
            services.AddScoped<AuthorType>();
            services.AddScoped<BookType>();
            services.AddScoped<LibraryQuery>();
            services.AddScoped<ISchema, LibrarySchema>(provider => new LibrarySchema(provider));
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddGraphQL(x => x.AddSchema<LibrarySchema>().AddNewtonsoftJson());
        }
    }
}
