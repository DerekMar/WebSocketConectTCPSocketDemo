using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebSocketWinForm.SocketClient
{
    class SocketClientClass
    {

        public WebSocketSessionManager _Session = null;//控制websocket的客户端

        public string _id; //websocket的唯一标识符

        private Socket clientSocket = null;//socketclient客户端，用于连接服务端

        public bool isReceiveMsg = true;//是否接收消息

        public SocketClientClass(WebSocketSessionManager Session, string ID) {
            _Session = Session;
            _id = ID;
        }
        /// <summary>
        /// 打开客户端
        /// </summary>
        public void init()
        {
             //设定服务器IP地址  
            string IP = System.Configuration.ConfigurationSettings.AppSettings["IP"];
            IPAddress ip = IPAddress.Parse(IP);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                int Port = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["Port"]);
                clientSocket.Connect(new IPEndPoint(ip, Port)); //配置服务器IP与端口  
                
                //通知websocket客户端
                if (_id != null && _Session != null) 
                {
                    IWebSocketSession iwss;
                    if (_Session.TryGetSession(_id, out iwss))
                    {
                        _Session.SendTo("连接SocketClient成功", _id);
                    }
                    //TODO 其它操作
                }
            }
            catch
            {
                return;
            }

            //通过 clientSocket 发送数据  
            try
            {
                string sendMessage = string.Format("hello world {0} in {1}", this._id, DateTime.Now.ToLongDateString());

                clientSocket.Send(Encoding.Default.GetBytes(sendMessage), SocketFlags.None);

                if (_id != null && _Session != null)
                {
                    IWebSocketSession iwss;
                    if (_Session.TryGetSession(_id, out iwss))
                    {
                        if (iwss.State == WebSocketSharp.WebSocketState.Open) 
                        {
                            _Session.SendTo("发送消息ToSocketClient：" + sendMessage, _id);
                        }
                    } 
                }
            }
            catch
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            //通过clientSocket接收数据
            while (isReceiveMsg)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int receiveLength = clientSocket.Receive(result);
                    //实际接收到的有效字节数
                    if (receiveLength != 0)
                    {
                        if (_id != null && _Session != null)
                        {
                            IWebSocketSession iwss;
                            if (_Session.TryGetSession(_id, out iwss))
                            {
                                if (iwss.State == WebSocketSharp.WebSocketState.Open)
                                {
                                    _Session.SendTo("SocketClient：" + Encoding.Default.GetString(result, 0, receiveLength), _id);
                                }
                            } 
                        }
                    }
                }
                catch
                {
                    //if (clientSocket != null) 
                    //{
                    //    clientSocket.Shutdown(SocketShutdown.Both);
                    //    clientSocket.Close();
                    //}   
                    break;
                }
            }
            Console.WriteLine("socket 结束");
        }
        public void close()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }
}
