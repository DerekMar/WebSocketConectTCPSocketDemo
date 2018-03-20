using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketWinForm.Handle
{
    interface IHandle
    {
        //用于接收消息
        void accept(string msg);
    }
}
