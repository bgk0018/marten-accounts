using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.CorrelationId
{
    public class CorrelationIdMiddleware
    {
        readonly RequestDelegate next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        readonly Func<string, Action<HttpContext>> _matchFunc = id =>
        {
            if (id == null)
                return context =>
                    context.Request.Headers.Add("X-Correlation-ID", Guid.NewGuid().ToString());

            if (Guid.TryParse(id, out _))
                return _ => { };

            return _ =>
                throw new ValidationException(
                    "X-Correlation-ID was not in a parseable format, please verify you're sending a UUID.");
        };

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["x-correlation-id"].FirstOrDefault();

            _matchFunc(correlationId)(context);

            await next(context);
        }
    }

    public static class CorrelationIdExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
