namespace MigrationService.Models
{
	public class RequestSqsModel
	{
		public string MessageId { get; set; }
		public string MessageGroupId { get; set; }
		public string MessageDeduplicationId { get; set; }
		public string Body { get; set; }

	}

	public class SqsMessageBodyModel
	{
		public int RequestId { get; set; }
		public string TargetDatabase { get; set; }
		public int Count { get; set; }

	}
}
