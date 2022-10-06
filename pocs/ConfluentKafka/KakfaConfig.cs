namespace ConfluentKafka
{
	public class KakfaConfig
	{
		public string GroupId { get; set; }
		public string Topic { get; set; }
		public string BootstrapServers { get; set; }
		public string AutoOffsetReset { get; set; }
		public string SecurityProtocol { get; set; }
		public string SaslMechanism { get; set; }
		public string SaslUsername { get; set; }
		public string SaslPassword { get; set; }
	}
}
