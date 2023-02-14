namespace WineryStore.API.Middleware
{
	public class Lifetime : ILifetimeTransient, ILifetimeScoped, ILifetimeSingleton
	{
		public string OperationId { get; }

	    public Lifetime()
		{
			OperationId = Guid.NewGuid().ToString()[^4..];
		}	
	}
}
