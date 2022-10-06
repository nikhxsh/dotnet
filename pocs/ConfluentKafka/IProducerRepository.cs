using Confluent.Kafka;

namespace ConfluentKafka
{
	public interface IProducerRepository
	{
		Task TryCreateTopic(string bootstrapServers, string topicName, CancellationToken token);
		Task TryProduceMessages(ProducerConfig producerConfig, string topic, List<ProductModel> products, CancellationToken token);
	}
}
