using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace ProducerGennericDataMigration.Repository;

public class SQSRepositroy
{
    private readonly AmazonSQSClient _awssqsClient;
    private readonly SQS_Setting _sqsSetting;

    public SQSRepositroy(SQS_Setting sqsSetting)
    {
        _awssqsClient = new AmazonSQSClient();
        _sqsSetting = sqsSetting;
    }

    public async Task<Amazon.SQS.Model.Message> GetMessageFromSqs()
    {
        var request = new ReceiveMessageRequest
        {
            QueueUrl = _sqsSetting.backGroundServiceSQSString,
            MaxNumberOfMessages = 1,
            VisibilityTimeout = 60,
            WaitTimeSeconds = 0,
        };

        var receiveMessage =  await _awssqsClient.ReceiveMessageAsync(request);
        var sqsMessage = receiveMessage.Messages.FirstOrDefault();

        return sqsMessage;
    }

    public async Task SendMessagetoBatchSQS(List<BatchSQSModel> batchSqsModels)
    {
        foreach (var batchSqsModel in batchSqsModels)
        {
            var request = new SendMessageRequest()
            {
                MessageBody = JsonSerializer.Serialize(batchSqsModel),
                MessageGroupId = "1",
                MessageDeduplicationId = Guid.NewGuid().ToString()
            };
            if (batchSqsModel.TargetDatabase == "postgres")
            {
                request.QueueUrl = _sqsSetting.postgreSQSString;
            }
            else if (batchSqsModel.TargetDatabase == "mongo")
            {
                request.QueueUrl = _sqsSetting.mongoSQSString;
            }

            var response = await _awssqsClient.SendMessageAsync(request);
        }
        

    }

    public async Task DeleteMessageFromSQS(string recieptHandle)
    {
        var deleteRequest = new DeleteMessageRequest()
        {
            QueueUrl = _sqsSetting.backGroundServiceSQSString,
            ReceiptHandle = recieptHandle
        };

        var deleteResponse = await _awssqsClient.DeleteMessageAsync(deleteRequest);
    }
}