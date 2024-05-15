using Library.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Library.API.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public IWebHostEnvironment Environment { get; }
        public ILogger<JsonExceptionFilter> Logger { get; }

        public JsonExceptionFilter(IWebHostEnvironment env, ILogger<JsonExceptionFilter> logger)
        {
            Environment = env;
            Logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var error = new ApiError();
            if (Environment.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.ToString();
            }
            else
            {
                error.Message = "服務器出錯";
                error.Detail = context.Exception.Message;
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"服務發生異常: {context.Exception.Message}");
            sb.AppendLine(context.Exception.ToString());
            Logger.LogCritical(sb.ToString());
        }
    }
}