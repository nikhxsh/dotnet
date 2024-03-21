namespace WineryAPI.Middleware
{
	public interface ILifetime
	{
		string OperationId { get; }
	}

	public interface ILifetimeTransient : ILifetime { }
	public interface ILifetimeScoped : ILifetime { }
	public interface ILifetimeSingleton : ILifetime { }
}
