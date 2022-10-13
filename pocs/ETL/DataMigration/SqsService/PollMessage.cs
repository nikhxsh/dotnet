using Amazon.SQS;
using Amazon.SQS.Model;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqsService
{
    public class PollMessage
    {
        private readonly AmazonSQSClient _amazonSQSClient;
        private List<DeleteMessageBatchRequestEntry> deleteEntries;
        private string _queueUrl;
        public PollMessage()
        {
            _amazonSQSClient = new AmazonSQSClient();
        }

        public async Task<List<MessageInfo>> FetchMessageFromSqsAsync(string queueName)
        {
            List<MessageInfo> receivedMessagesList = new List<MessageInfo>();
            try
            {
                deleteEntries = new List<DeleteMessageBatchRequestEntry>();

                var request = new GetQueueUrlRequest
                {
                    QueueName = queueName,
                };

                _queueUrl = _amazonSQSClient.GetQueueUrlAsync(request).Result.QueueUrl;

                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 1
                };

                var receiveMessageResponse = await _amazonSQSClient.ReceiveMessageAsync(receiveMessageRequest);

                foreach (var message in receiveMessageResponse.Messages)
                {
                    //var messageBody = JsonSerializer.Deserialize<MessageBody>(message.Body);
                    receivedMessagesList.Add(new MessageInfo
                    {
                        Id = message.MessageId,
                        Body = message.Body,
                        ReceiptHandle = message.ReceiptHandle
                    });

                    deleteEntries.Add(new DeleteMessageBatchRequestEntry
                    {
                        Id = message.MessageId,
                        ReceiptHandle = message.ReceiptHandle
                    });
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return receivedMessagesList;
        }

        /// <summary>
        /// Deletes Messages from SQS
        /// </summary>
        public async Task DeleteMessageAsync(string messageId)
        {
            var entry = deleteEntries.Find(x => x.Id == messageId);
           
            var deleteRequest = new DeleteMessageRequest()
            {
                QueueUrl = _queueUrl,
                ReceiptHandle = entry.ReceiptHandle
            };

            var deleteResponse = await _amazonSQSClient.DeleteMessageAsync(deleteRequest);
        }
    }
}
