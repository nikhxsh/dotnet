using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using Storage;
using Storage.Datastore;
using WineryStore.API.Middleware;
using WineryStore.API.Filters;

namespace RestService
{
	public class Startup
	{
		readonly string WineryAllowSpecificOrigins = "_wineryAllowSpecificOrigins";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		/// <summary>
		/// - IServiceCollection is an interface provided by the Microsoft.Extensions.DependencyInjection namespace that is 
		///   used to define a container for managing dependencies in an application. 
		/// - It is a part of the .NET Core dependency injection framework and is commonly used in ASP.NET Core web applications
		/// - By using IServiceCollection, developers can decouple the dependencies between classes and promote loose coupling, which 
		///   can lead to better testability, maintainability, and scalability of their applications
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			// Register services with DI
			services.AddCors(options =>
			{
				options.AddPolicy(WineryAllowSpecificOrigins, builder =>
				{
					builder
					.WithOrigins("http://localhost:3000", "http://localhost:4200")
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});

			//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0
			//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1

			services.AddScoped<IWineryRepository, WineryRepository>();
			services.AddScoped<IWineRepository, WineRepository>();
			services.AddScoped<IWineryDataStore, WineryDataStore>();
			services.AddScoped<IWineDataStore, WineDataStore>();

			// Transient objects are always different. The transient OperationId value is different in the IndexModel and in the middleware.
			// - It creates a new instance every time when the client asks for it (More likely a constructor)
			// - E.g. services.AddTransient<IDataAccess, DataAccess>();
			//	 will return a new DataAccess object every time a client code asks for it
			// - Transient would be used when the component cannot be shared, so a non-thread-safe database access object would be one example
			services.AddTransient<ILifetimeTransient, Lifetime>();

			// Scoped objects are the same for a given request but differ across each new request.
			// - Creates a new instance for each http web request
			// - E.g. services.AddScoped<IShoppingCart, ShoppingCart>();
			//	 this mean each web request will be having its own shopping cart instance which intern means each user / client will be having 
			//   its own shoping cart instance for that http web request
			services.AddScoped<ILifetimeScoped, Lifetime>();

			// Singleton objects are the same for every request.
			// - Create single instance for all the http web requests
			// - Singleton components are shared always, so they are best for thread-safe components that do not need to be bound to a request. 
			// - An example would be IOptions, which gives access to configuration settings.
			// - An HttpClient wrapper class that uses SendAsync on a single static HttpClient instance would also be completely thread-safe, 
			//   and a good candidate for being a Singleton 
			services.AddSingleton<ILifetimeSingleton, Lifetime>();

			//Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.1.1
			services.AddDbContext<Storage.EF.Datastore.WineryContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("WineryConnection")
			));

			services
				.AddMvc(options =>
				{
					options.Filters.Add(new LogActionFilter());
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		// https://www.tutorialsteacher.com/core/aspnet-core-middleware
		// Defines a class that provides the mechanisms to configure an application's request pipeline
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.Use(async (context, next) =>
			{
				//Thread.Sleep(1500);
				await next.Invoke();
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseCors(WineryAllowSpecificOrigins);
			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseServiceLifetimeMiddleware();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
			});
		}
	}

	public class DesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
	{
		public T CreateDbContext(string[] args)
		{
			IConfigurationRoot configurationRoot = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var builder = new DbContextOptionsBuilder<T>();
			builder.UseSqlServer(configurationRoot.GetConnectionString("WineryConnection"));

			var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options);
			return dbContext;
		}
	}

	public class WineryDesignTimeDbContextFactory : DesignTimeDbContextFactory<Storage.EF.Datastore.WineryContext> { }

}