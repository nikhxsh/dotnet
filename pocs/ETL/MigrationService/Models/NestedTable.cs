

using System.Text.Json.Serialization;

namespace MigrationService.Models
{
	public class NestedTable : BaseTable
	{
		public string ObjectIdentifier { get; set; }

		[JsonPropertyName("skey")]
		public string SourceKey { get; set; }

		[JsonPropertyName("tkey")]
		public string TargetKey { get; set; }

		public NestedTable Nest { get; set; } = null;
	}
}
