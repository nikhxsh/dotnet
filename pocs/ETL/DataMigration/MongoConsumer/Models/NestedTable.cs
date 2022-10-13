using System.Text.Json.Serialization;

namespace MongoConsumer.Models
{
	public class NestedTable : BaseTable
	{
		public string objectIdentifier { get; set; }

		[JsonPropertyName("skey")]
		public string skey { get; set; }

		[JsonPropertyName("tkey")]
		public string tkey { get; set; }

		public NestedTable Nest { get; set; } = null;
	}
}
