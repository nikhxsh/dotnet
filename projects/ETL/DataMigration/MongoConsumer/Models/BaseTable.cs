namespace MongoConsumer.Models
{
	public class BaseTable
	{
		public string tableName { get; set; }
		public string[] select { get; set; }
		public string conditions { get; set; }
		public string key { get; set; }
	}
}
