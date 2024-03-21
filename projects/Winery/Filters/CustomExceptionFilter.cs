using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WineryStore.API.Filters
{
	/// <summary>
	/// - Can be used to implement common error handling policies
	/// - Exception filters are used to deal with any exceptions that may arise throughout the pipeline
	/// - You can leverage exception filters to apply global policies to unhandled exceptions in your application and to execute custom code 
	///   when an exception has occurred
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class CustomExceptionFilter : Attribute, IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			context.Result = new ViewResult()
			{
				StatusCode = (int)HttpStatusCode.BadRequest,
				ViewName = "Error"
			};
			context.ExceptionHandled = true;
		}
	}
}
