using Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Diagnostics;

namespace WineryStore.API.Filters
{
	/// <summary>
	/// - The ASP.NET MVC framework supports four different types of filters
	///	 - Authorization filters – Implements the IAuthorizationFilter attribute.
	///	 - Action filters – Implements the IActionFilter attribute.
	///	 - Result filters – Implements the IResultFilter attribute.
	///	 - Exception filters – Implements the IExceptionFilter attribute
	///	- Filters are executed in the order listed above
	///	 
	/// 
	/// - Action filters contain logic that is executed before and after a controller action executes. You can use an action filter, 
	///   for instance, to modify the view data that a controller action returns
	/// </summary>
	public class LogActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			Log("OnActionExecuting", filterContext.RouteData);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			Log("OnActionExecuted", filterContext.RouteData);
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			Log("OnResultExecuting", filterContext.RouteData);
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			Log("OnResultExecuted", filterContext.RouteData);
		}

		private void Log(string methodName, RouteData routeData)
		{
			var controllerName = routeData.Values["controller"];
			var actionName = routeData.Values["action"];
			var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
			Debug.WriteLine(message, "Action Filter Log");
		}
	}
}
