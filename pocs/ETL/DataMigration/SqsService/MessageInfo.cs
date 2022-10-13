using System.Collections.Generic;

namespace SqsService
{
    public class MessageInfo { 
        public string Id { get; set; }
        public string Body { get; set; }
        public string ReceiptHandle { get; set; }
    }
}
