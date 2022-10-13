namespace ProducerGennericDataMigration;

public class SqsMessageBodyModel
{
    public int RequestId { get; set; }
    public string TargetDatabase { get; set; }
    public int Count { get; set; }
}