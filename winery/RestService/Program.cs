using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace RestService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		/// <summary>
		/// - ASP.NET Core apps configure and launch a host. The host is responsible for app startup and lifetime management. 
		/// - At a minimum, the host configures a server and a request processing pipeline. The host can also set up logging, 
		///   dependency injection, and configuration
		/// - https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-7.0
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			 WebHost.CreateDefaultBuilder(args)
				  .UseStartup<Startup>();
	}
}
