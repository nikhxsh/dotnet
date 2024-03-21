using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WineryStore.API.Filters
{
	/// <summary>
	/// - Only Authorization filters run before resource filters
	/// - Resource filters are useful to short-circuit most of the pipeline. For example, a caching filter can avoid 
	///   the rest of the pipeline on a cache hit
	/// - You can take advantage of a resource filter to implement caching or to short-circuit the filter pipeline
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class CustomResourceFilterAttribute : Attribute, IResourceFilter
	{
		public void OnResourceExecuted(ResourceExecutedContext context)
		{
			context.Result = new ContentResult()
			{
				Content = "This is a Resource filter."
			};
		}

		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			throw new NotImplementedException();
		}
	}
}
