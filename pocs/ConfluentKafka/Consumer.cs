using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace ConfluentKafka
{
	public class Consumer : IHostedService
	{
		private readonly KakfaConfig config;

		public Consumer(IOptions<KakfaConfig> config)
		{
			this.config = config.Value;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var consumerConfig = new ConsumerConfig
			{
				GroupId = config.GroupId,
				AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), config.AutoOffsetReset),
				BootstrapServers = config.BootstrapServers,
				SecurityProtocol = (SecurityProtocol)Enum.Parse(typeof(SecurityProtocol), config.SecurityProtocol),
				SaslMechanism = (SaslMechanism)Enum.Parse(typeof(SaslMechanism), config.SaslMechanism),
				SaslUsername = config.SaslUsername,
				SaslPassword = config.SaslPassword
			};

			CancellationTokenSource cts = new();
			Console.CancelKeyPress += (_, e) =>
			{
				e.Cancel = true; // prevent the process from terminating.
				cts.Cancel();
			};

			Parallel.For(0, 6, index =>
			{
				Consume(consumerConfig, index, cts);
			});

			return Task.CompletedTask;
		}

		/// <summary>
		///  - each partition is consumed by exactly one consumer in the group
		///  - the number of consumer processes in a group must be <= number of partitions
		/// </summary>
		/// <param name="consumerConfig"></param>
		/// <param name="consumerName"></param>
		/// <param name="cts"></param>
		private void Consume(ConsumerConfig consumerConfig, int index, CancellationTokenSource cts)
		{
			using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
			{
				//consumer.Subscribe(config.Topic); // consume from an default, assigned partition
				consumer.Assign(new TopicPartition(config.Topic, new Partition(index))); // consume from an specific partition
				try
				{
					while (true)
					{
						var cr = consumer.Consume(cts.Token);
						Console.WriteLine($"Consumer {index}, Topic {config.Topic}, Partition {cr.Partition.Value}, Key {cr.Message.Key} & Message {cr.Message.Value}");
					}
				}
				catch (OperationCanceledException)
				{
					// Ctrl-C was pressed.
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				finally
				{
					consumer.Close();
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
