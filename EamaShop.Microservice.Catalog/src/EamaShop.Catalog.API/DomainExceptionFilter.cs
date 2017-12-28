using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace EamaShop.Merchant.API
{
    /// <summary>
    /// A handing filter that runs after an action has thrown an <see cref="DomainException"/>
    /// </summary>
    internal class DomainExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DomainExceptionFilter> _logger;
        /// <summary>
        /// Initiaze a new <see cref="DomainExceptionFilter"/> instance.
        /// </summary>
        /// <param name="logger"></param>
        public DomainExceptionFilter(ILogger<DomainExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        ///<inheritdoc />
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled || !(context.Exception is DomainException))
            {
                return;
            }

            _logger.LogWarning(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            context.Result = new ObjectResult(new { context.Exception.Message })
            {
                StatusCode = 400
            };
            context.ExceptionHandled = true;
        }
    }
}