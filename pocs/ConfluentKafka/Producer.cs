using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace ConfluentKafka
{
	public class Producer : IHostedService
	{
		private readonly KakfaConfig config;
		private IProducerRepository producerRepository;

		public Producer(IOptions<KakfaConfig> config, IProducerRepository producerRepository)
		{
			this.config = config.Value;
			this.producerRepository = producerRepository;	
		}

		/// <summary>
		/// - Kafka guarantees the order of messages within a partition
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			// - All the records with the same key will arrive in the same Kafka Partition
			var products = new List<ProductModel>
			{
				new ProductModel { Id = "1009", Name = "Orange", Price = 30.5 },
				new ProductModel { Id = "1011", Name = "Apple", Price = 50.5 },
				new ProductModel { Id = "1022", Name = "Coke", Price = 10.5 },
				new ProductModel { Id = "1003", Name = "Burger", Price = 15.5 },
				new ProductModel { Id = "1001", Name = "Fries", Price = 11.5 },
				new ProductModel { Id = "1004", Name = "Kela", Price = 12.5 }
			};

			var producerConfig = new ProducerConfig
			{
				BootstrapServers = config.BootstrapServers,
				SecurityProtocol = (SecurityProtocol)Enum.Parse(typeof(SecurityProtocol), config.SecurityProtocol),
				SaslMechanism = (SaslMechanism)Enum.Parse(typeof(SaslMechanism), config.SaslMechanism),
				SaslUsername = config.SaslUsername,
				SaslPassword = config.SaslPassword
			};

			// Try and create the delay topic (for failed requests)
			await producerRepository.TryCreateTopic(config.BootstrapServers, "dev.ims.retry", cancellationToken);

			// Create the deadletter topic (for failed retry requests)
			await producerRepository.TryCreateTopic(config.BootstrapServers, "dev.ims.dlq", cancellationToken);

			// Produce messages
			await producerRepository.TryProduceMessages(producerConfig, config.Topic, products, cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
