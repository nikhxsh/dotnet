using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqsService
{
    public class MessageBody
    {  
        public int BatchId { get; set; }
        public int RequestId { get; set; }
        public string TargetDatabase { get; set; }
        public bool IsLastBatch { get; set; }
        public int top { get; set; }
        public int Skip { get; set; }
        public string Status { get; set; }
    }
}
