using Asp.Versioning;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Helpers;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson()
    .AddXmlSerializerFormatters()
    .AddMvcOptions(config =>
    {
        config.Filters.Add<JsonExceptionFilter>();

        config.CacheProfiles.Add("Default",
            new CacheProfile()
            {
                Duration = 60
            });

        config.CacheProfiles.Add("Never",
            new CacheProfile()
            {
                Location = ResponseCacheLocation.None,
                NoStore = true
            });
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<CheckAuthorExistFilterAttribute>();

builder.Services.AddDbContext<LibraryDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(LibraryMappingProfile));

builder.Services.AddResponseCaching(options =>
{
    options.UseCaseSensitivePaths = true;
    options.MaximumBodySize = 1024;
});

builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Caching:Host"];
    options.InstanceName = builder.Configuration["Caching:Instance"];
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1.0);
    options.ReportApiVersions = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("ver"),
        new QueryStringApiVersionReader("api-version"),
        new MediaTypeApiVersionReader());
})
    .AddMvc(options =>
    {
        var controllerConvention = options.Conventions.Controller<Library.API.Controllers.VersionTest.V1.ProjectController>();
        controllerConvention.HasApiVersion(new ApiVersion(1, 0));
        controllerConvention.HasDeprecatedApiVersion(new ApiVersion(1, 0));

        options.Conventions.Controller<Library.API.Controllers.VersionTest.V2.ProjectController>()
            .HasApiVersion(new ApiVersion(2, 0));
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library.API", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Library.API", Version = "v2" });
});

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Library.API v1");
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"Library.API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching();

app.MapControllers();

app.Run();
