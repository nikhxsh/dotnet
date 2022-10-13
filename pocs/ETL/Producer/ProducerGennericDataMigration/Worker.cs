using ProducerGennericDataMigration.Repository;

namespace ProducerGennericDataMigration;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private SQL_Setting _sqlSetting;
    private SQS_Setting _sqsSetting;
    private MessageRepository _messageRepository;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _sqlSetting = new SQL_Setting();
        _sqlSetting.sqlConnectionString= "Host=ims-rds.cdmnj4oncsro.us-west-2.rds.amazonaws.com;Username=postgres;Password=IMSHACK1;Database=postgres";
        _sqsSetting = new SQS_Setting() ;
        _sqsSetting.mongoSQSString = "https://sqs.us-west-2.amazonaws.com/794344549935/Mongo_IMS_Queue.fifo";
        _sqsSetting.postgreSQSString = "https://sqs.us-west-2.amazonaws.com/794344549935/Postgres_IMS_Queue.fifo";
        _sqsSetting.backGroundServiceSQSString = "https://sqs.us-west-2.amazonaws.com/794344549935/backGround_IMS_Queue.fifo";
        _messageRepository = new MessageRepository(_sqlSetting,_sqsSetting);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _messageRepository.ProcessMessage();
            await Task.Delay(60000, stoppingToken);
        }
    }
}