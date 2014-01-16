using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace WCFServiceTester.Messaging
{
    public class StatusMessage: MessageBase
    {

        public StatusMessage()
        {
            IsIndeterminate = true;
            PercentComplete = 0;
        }
        public StatusMessage(string statusMessage, int percentComplete, bool isIndeterminate)
        {
            Message = statusMessage;
            PercentComplete = percentComplete;
            IsIndeterminate = isIndeterminate;
        }

        public string Message { get; set; }
        public int PercentComplete { get; set; }
        public bool IsIndeterminate { get; set; }
    }
}
