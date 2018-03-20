using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketWinForm.SocketClient;

namespace WebSocketWinForm.Applications
{
    public delegate void BoilerLogHandler(string str);
    public delegate void BoilerUserListHandler(string str);

    class ThreadClass 
    {
        public Thread thread;
        public SocketClientClass socketclient;

        public ThreadClass(Thread t, SocketClientClass s)
        {
            thread = t;
            socketclient = s;
        }
    }
    class ConnectDataBase: WebSocketBehavior
    {
        private static int _number = 0;
        private System.Timers.Timer t = new System.Timers.Timer(10000);


        public event BoilerLogHandler BoilerEventLog;
        public event BoilerUserListHandler BoilerEventUserList;


        private static List<ThreadClass> _threadpool = new List<ThreadClass>();//简单的线程池列表
        public ConnectDataBase() { 
           
        }
        /// <summary>
        /// 从websocket请求获取参数，获取name的参数值
        /// </summary>
        /// <returns></returns>
        private string getName()
        {
            var name = Context.QueryString["name"];
            return !name.IsNullOrEmpty() ? name : "";
        }
        private string getPassword() {
            var password = Context.QueryString["pwd"];
            return !password.IsNullOrEmpty() ? password : "";
        }
        /// <summary>
        /// 获取当前已经连接的websocket数
        /// </summary>
        /// <returns></returns>
        private static int getNumber()
        {
            return Interlocked.Increment(ref _number);
        }
        /// <summary>
        /// 接收到消息的回掉函数
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMessage(MessageEventArgs e)
        {
            this._OnMessage(e.Data);
        }
        /// <summary>
        /// 打开时的回调函数
        /// </summary>
        protected override void OnOpen()
        {
            this._OnOpen();
        }
        /// <summary>
        /// 关闭时的回掉函数
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClose(CloseEventArgs e)
        {
            this._OnClose();
        }
        /// <summary>
        /// 发生错误的回掉函数
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(ErrorEventArgs e)
        {
            this._OnError();
        }
        /// <summary>
        /// 内存触发打开时间的处理函数
        /// </summary>
        private void _OnOpen() 
        {
            //发送消息返回给客户端,表示连接成功
            Send(string.Format("{0}:连接成功", getName()));
            //登陆获取参数
            string username = getName();
            string password = getPassword();

            try
            {
                SocketClient.SocketClientClass socketclient = new SocketClient.SocketClientClass(Sessions, ID);
                Thread aThread = new Thread(new ThreadStart(socketclient.init));
                _threadpool.Add(new ThreadClass(aThread, socketclient));
                aThread.Start();
            }
            catch (Exception ex) {
                
            }

            //设置窗体日志
            if (BoilerEventUserList != null)
            {
                BoilerEventUserList(ID);
            }
            //实例化Timer类，设置间隔时间为10000毫秒；保持一种心跳
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }
        /// <summary>
        /// 内部触发关闭的处理事件
        /// </summary>
        private void _OnClose()
        {
            //结束保持心跳的函数，防止一直继续,这个必须一开始就停掉
            //t.Stop();
            //广播所有的连接的client
            //Sessions.Broadcast(String.Format("{0} got logged off...", _name));
            string username = getName();
            string password = getPassword();

            for (int i = 0; i < _threadpool.Count; i++)
            {
                if (_threadpool[i].socketclient._id == ID)
                {
                    //结束socketclient接收消息
                    _threadpool[i].socketclient.isReceiveMsg = false;
                    //释放socketclient对象
                    _threadpool[i].socketclient.close();
                    //销毁线程
                        _threadpool[i].thread.Abort();
                    //移除对象池
                    _threadpool.RemoveAt(i);
                    break;
                }
            }
          
            //设置窗体的日志
            if (BoilerEventLog != null)
            {
                BoilerEventLog(String.Format("{0}: {1}", getName(), "断开连接"));
            }
        }
        /// <summary>
        /// 内部触发接收到消息的处理事件
        /// </summary>
        private void _OnMessage(string msg)
        {

            if (BoilerEventLog != null)
            {
                BoilerEventLog(String.Format("{0}: {1}", getName(), msg));
            }

        }
        /// <summary>
        /// 内部触发错误的处理事件
        /// </summary>
        private void _OnError() 
        {
            t.Stop();
        }
        ///// <summary>
        ///// 每十秒的触发一次的心跳函数
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="e"></param>
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            string replyTxt = "保持连接";
            Send(replyTxt);
        }
      
    }
}
