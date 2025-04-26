using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class CashAttribute(int durationInSeconds) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cashService = context.HttpContext.RequestServices.GetRequiredService<IServiceManger>().cashService;
            var cashKey = GenerateCashKey(context.HttpContext.Request);
            var result = await cashService.GetCashValueAsync(cashKey);
            if (!string.IsNullOrEmpty(result))
            {
                // Return Response 
                context.Result = new ContentResult() { 
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK,
                Content = result
                };
                return ;
            }
            // Execute End Point 
            var contextResult = await next.Invoke();
            if (contextResult.Result is OkObjectResult okObject)
            {
                await cashService.SetCashValueAsync(cashKey ,okObject.Value , TimeSpan.FromSeconds( durationInSeconds));
            }

        }

        private string GenerateCashKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString();  
        }

    }
}
