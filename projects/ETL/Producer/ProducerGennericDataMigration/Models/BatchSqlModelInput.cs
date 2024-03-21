namespace ProducerGennericDataMigration;

public class BatchSqlModelInput
{
    public int RequestId { get; set; }
    public bool IsLastBatch { get; set; }
    public int top { get; set; }
    public int Skip { get; set; }
    public string Status { get; set; }
}