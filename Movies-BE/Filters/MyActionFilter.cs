using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Movies_BE.Filters
{
    public class MyActionFilter : IActionFilter
    {

        private readonly ILogger<MyActionFilter> logger;
        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Before Execute Action");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("After Execute Action");
        }

       
    }
}
