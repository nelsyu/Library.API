﻿using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.API.Filters
{
    public class CheckAuthorExistFilterAttribute : ActionFilterAttribute
    {
        public IRepositoryWrapper RepositoryWrapper { get; }

        public CheckAuthorExistFilterAttribute(IRepositoryWrapper repositoryWrapper)
        {
            RepositoryWrapper = repositoryWrapper;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authorIdParameter = context.ActionArguments.Single(m => m.Key == "authorId");
            Guid authorId = (Guid)authorIdParameter.Value!;

            var isExist = await RepositoryWrapper.Author.IsExistAsync(authorId);
            if (!isExist)
            {
                context.Result = new NotFoundResult();
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}