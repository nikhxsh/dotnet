namespace ProducerGennericDataMigration;

public class BatchSQSModel
{
    public int BatchId { get; set; }
    public int RequestId { get; set; }
    public string TargetDatabase { get; set; }
    public bool IsLastBabtch { get; set; }
    public int top { get; set; }
    public int Skip { get; set; }
    public string Status { get; set; }
}