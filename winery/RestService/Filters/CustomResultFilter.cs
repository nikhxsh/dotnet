using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WineryStore.API.Filters
{
	/// <summary>
	/// - Their execution surrounds the execution of action results
	/// - Result filters are only executed when an action or action filter produces an action result. Result filters are 
	///   not executed when:
	///   - An authorization filter or resource filter short-circuits the pipeline
	///   - An exception filter handles an exception by producing an action result
	/// </summary>
	public class CustomResultFilter : Attribute, IResultFilter
	{
		public void OnResultExecuted(ResultExecutedContext context)
		{
		}

		public void OnResultExecuting(ResultExecutingContext context)
		{
			context.Result = new ViewResult
			{
				ViewName = "Hello"
			};
		}
	}
}
