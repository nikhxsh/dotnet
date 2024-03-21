using WineryAPI.Middleware;
using WineryAPI.Service;
using WineryAPI.Storage.Dapper;
using WineryAPI.Storage.Models;
using WineryAPI.Storage.Repository;
using WineryStore.API.Filters;

string WineryAllowSpecificOrigins = "_wineryAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure cors
builder.Services.AddCors(options =>
{
	options.AddPolicy(WineryAllowSpecificOrigins, builder =>
	{
		builder
		.WithOrigins("http://localhost:3000", "http://localhost:4200")
		.AllowAnyHeader()
		.AllowAnyMethod();
	});
});

// Add services to the container.
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
builder.Services.AddSingleton<IDapperContext, SqlDapperContext>();
builder.Services.AddScoped<IRepository<WineDTO>, SqlRepository<WineDTO>>();
builder.Services.AddScoped<IRepository<WineryDTO>, SqlRepository<WineryDTO>>();
builder.Services.AddScoped<IWineService, WineService>();

// Transient objects are always different. The transient OperationId value is different in the IndexModel and in the middleware.
// - It creates a new instance every time when the client asks for it (More likely a constructor)
// - E.g. services.AddTransient<IDataAccess, DataAccess>();
//	 will return a new DataAccess object every time a client code asks for it
// - Transient would be used when the component cannot be shared, so a non-thread-safe database access object would be one example
builder.Services.AddTransient<ILifetimeTransient, Lifetime>();

// Scoped objects are the same for a given request but differ across each new request.
// - Creates a new instance for each http web request
// - E.g. services.AddScoped<IShoppingCart, ShoppingCart>();
//	 this mean each web request will be having its own shopping cart instance which intern means each user / client will be having 
//   its own shoping cart instance for that http web request
builder.Services.AddScoped<ILifetimeScoped, Lifetime>();

// Singleton objects are the same for every request.
// - Create single instance for all the http web requests
// - Singleton components are shared always, so they are best for thread-safe components that do not need to be bound to a request. 
// - An example would be IOptions, which gives access to configuration settings.
// - An HttpClient wrapper class that uses SendAsync on a single static HttpClient instance would also be completely thread-safe, 
//   and a good candidate for being a Singleton 
builder.Services.AddSingleton<ILifetimeSingleton, Lifetime>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
	options.Filters.Add(new LogActionFilter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(WineryAllowSpecificOrigins);

app.UseServiceLifetimeMiddleware();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
