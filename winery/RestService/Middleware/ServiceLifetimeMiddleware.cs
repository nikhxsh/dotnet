using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WineryStore.API.Middleware
{
	public class ServiceLifetimeMiddleware
	{
		private readonly RequestDelegate _next;
        private readonly ILifetimeTransient _transientOperation;
        private readonly ILifetimeSingleton _singletonOperation;

        public ServiceLifetimeMiddleware(RequestDelegate next,
            ILifetimeTransient transientOperation,
            ILifetimeSingleton singletonOperation)
        {
            _transientOperation = transientOperation;
            _singletonOperation = singletonOperation;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILifetimeScoped scopedOperation)
        {
            Console.Write("Transient: " + _transientOperation.OperationId);
            Console.Write("Scoped: " + scopedOperation.OperationId);
            Console.Write("Singleton: " + _singletonOperation.OperationId);

            await _next(context);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseServiceLifetimeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServiceLifetimeMiddleware>();
        }
    }
}
