using System.Text.Json;
using ProducerGennericDataMigration.SqlRepository;

namespace ProducerGennericDataMigration.Repository;

public class MessageRepository
{
    private readonly ILogger<Worker> _logger;
    private readonly SQL_Setting _sqlSetting;
    private readonly SQS_Setting _sqsSetting;
    private SQSRepositroy _sqsRepositroy;
    private readonly int BatchSize;
    private BatchSqlHelper _batchSqlHelper;
    private MessageProcessRepository _messageProcessRepository;
    private RequestSqlHelper _requestSqlHelper;

    public MessageRepository(SQL_Setting sqlSetting, SQS_Setting sqsSetting)
    {
        _sqlSetting = sqlSetting;
        _sqsSetting = sqsSetting;
        BatchSize = 10;
        _messageProcessRepository = new MessageProcessRepository();
        _sqsRepositroy = new SQSRepositroy(_sqsSetting);
        _batchSqlHelper = new BatchSqlHelper(_sqlSetting.sqlConnectionString);
        _requestSqlHelper = new RequestSqlHelper(_sqlSetting.sqlConnectionString);
    }

    public async Task ProcessMessage()
    {
        try
        {
            var message = await _sqsRepositroy.GetMessageFromSqs();
            if (message != null)
            {
                var messageBody = message.Body;
                var RequestMessage = JsonSerializer.Deserialize<SqsMessageBodyModel>(messageBody);
                var batches = await _messageProcessRepository.process(RequestMessage, BatchSize);
                var SqsModels = await _batchSqlHelper.GetPostgresQueryResultAsync(batches);
                SqsModels = SqsModels.Select(x =>
                {
                    x.TargetDatabase = RequestMessage.TargetDatabase;
                    return x;
                }).ToList();
                await _sqsRepositroy.SendMessagetoBatchSQS(SqsModels);
                await _sqsRepositroy.DeleteMessageFromSQS(message.ReceiptHandle);
                await _requestSqlHelper.UpdateRequestAsync(RequestMessage.RequestId);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
    
}