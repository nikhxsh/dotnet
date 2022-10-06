namespace ConfluentKafka
{
	public class Program
	{
		public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices(ConfigureServices);

		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			services.AddHostedService<Producer>();
			services.AddHostedService<Consumer>();
			services.AddSingleton<IProducerRepository, ProducerRepository>();
			//services.Configure<KakfaConfig>(context.Configuration.GetSection("Kafka-local"));
			services.Configure<KakfaConfig>(context.Configuration.GetSection("Kafka-confluent"));
		}
	}
}