using Amazon.SQS;
using Amazon.SQS.Model;
using MigrationService.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace MigrationService.Repository
{
	public class SqsRepository
	{
        private readonly AmazonSQSClient _awssqsClient;
        private readonly string _queueUrl;
        public SqsRepository(string queueUrl)
		{
            _awssqsClient = new AmazonSQSClient();
            _queueUrl = queueUrl;
		}
        public async Task SendMessagetoBatchSQS(RequestSqsModel requestSqsModel)
        {
            var request = new SendMessageRequest()
                {
                    MessageBody = requestSqsModel.Body,
                    MessageGroupId = requestSqsModel.MessageGroupId,
                    MessageDeduplicationId = requestSqsModel.MessageDeduplicationId,
                    QueueUrl = _queueUrl,
                    
                };
                var response = await _awssqsClient.SendMessageAsync(request);
        }
    }
}
