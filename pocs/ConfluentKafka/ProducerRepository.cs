using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;

namespace ConfluentKafka
{
	public class ProducerRepository : IProducerRepository
    {
		public async Task TryCreateTopic(string bootstrapServers, string topicName, CancellationToken token)
        {
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = bootstrapServers
            };

            using (var adminClient = new AdminClientBuilder(adminConfig).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification
                    {
                        Name = topicName,
                        ReplicationFactor = 1,
                        NumPartitions = 1
                    }
                });
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }

		public async Task TryProduceMessages(ProducerConfig producerConfig, string topic, List<ProductModel> products, CancellationToken token)
		{
			try
			{
				using var producer = new ProducerBuilder<string, string>(producerConfig).Build();
				var numProduced = 0;
				for (int i = 0; i < products.Count; ++i)
				{
					var product = products[i];

					var topicPartition = new TopicPartition(topic, new Partition(i));

					await producer.ProduceAsync(topicPartition, new Message<string, string> { Key = product.Id, Value = JsonConvert.SerializeObject(product) });

					Console.WriteLine($"Produced event to topic {topic}: key = {product.Id,-10} value = {JsonConvert.SerializeObject(product)}");
					numProduced += 1;
				}

				producer.Flush(TimeSpan.FromSeconds(10));
				Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to deliver message: {ex.Message}");
			}
		}
	}
}
 