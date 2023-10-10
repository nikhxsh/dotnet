using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WineryStore.API.Middleware
{
	/// <summary>
	/// - Request delegates are used to build the request pipeline. The request delegates handle each HTTP request
	/// - Filters are used to add functionality that is specific to an action method or controller, whereas
	///   middleware is used to add functionality that is applied to all requests passing through the pipeline
	/// - Filters are executed in a specific order, based on their type (authorization filters, action filters, result filters, etc.),
	///   whereas middleware is executed in the order it is added to the pipeline
	/// - Filters can access and modify the action context and arguments, whereas middleware can only modify the HTTP context
	/// - Multiple filters can be applied to a single action method, whereas only one middleware can be applied to a single request
	/// - In general, if you need to add functionality that is specific to a controller or action method, then filters may be the best choice. 
	///   If you need to add functionality that applies to all requests passing through the pipeline, then middleware may be the best choice
	///   
	/// - Authentication: we can use middleware to handle user authentication, such as verifying user credentials and creating authentication
    ///   token
	/// </summary>
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
