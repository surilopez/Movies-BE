﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Movies_BE.Filters
{
    public class ExceptionFilter:ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }

    }
}
