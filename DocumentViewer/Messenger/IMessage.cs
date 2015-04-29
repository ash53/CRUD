using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Messenger
{
    public interface IMessage
    {
        string Header { get; set; }
        string Body { get; set; }
        void ValidateMessage(string message);
    }
}
