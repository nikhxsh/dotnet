using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WineryStore.API.Filters
{

	/// <summary>
	/// - Are the first filters run in the filter pipeline.
	/// - Control access to action methods.
	/// - Have a before method, but no after method.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class CustomHttpsOnlyFilter : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!context.HttpContext.Request.IsHttps)
			{
				context.Result = new ForbidResult();
			}
		}
	}
}
