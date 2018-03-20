using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketWinForm.Applications;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using WebSocketWinForm.SocketClient;

namespace WebSocketWinForm
{
    public partial class WebSocketDemo : Form
    {
        private WebSocketServer _wsServer = new WebSocketServer(System.Net.IPAddress.Any, 12010);
        private WebSocketServiceHost wssk;

        public WebSocketDemo()
        {
            InitializeComponent();
            try
            {
                ///初始化websocket
                Action<ConnectDataBase> initializer = new Action<ConnectDataBase>(getConnectDataBase);
                //设置等待时间
                _wsServer.WaitTime = TimeSpan.FromSeconds(2);

                _wsServer.AddWebSocketService<ConnectDataBase>("/ConnectDataBase", initializer);

                _wsServer.KeepClean = true;
                WebSocketServiceManager wsManager = _wsServer.WebSocketServices;
                _wsServer.Start();
            }
            catch (Exception ex) {
                
            }

            if (_wsServer.IsListening)
            {
                logTextBox.AppendText(String.Format("Listening on port {0}, and providing WebSocket services:\r\n", _wsServer.Port));
                foreach (var path in _wsServer.WebSocketServices.Paths)
                    logTextBox.AppendText(String.Format("- {0}\r\n", path));
            }
            ////初始化socket客户端
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, EventArgs e)
        {
            _wsServer.Stop();
            logTextBox.AppendText(String.Format("close \r\n"));
        }
        /// <summary>
        /// 点击发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_send_Click(object sender, EventArgs e)
        {
            string sendID = this.textBox_sendID.Text;
            
            _wsServer.WebSocketServices.TryGetServiceHost("/ConnectDataBase",out wssk);

            WebSocketSessionManager wssm = wssk.Sessions;
            IWebSocketSession iwss;
            wssm.TryGetSession(sendID, out iwss);

            wssm.SendTo(string.Format("{0}: 来自服务端的消息", "server"), sendID);
            //wssm.PingTo(string.Format("{0}: 来自服务端的消息", "server"), sendID);
            
        }
        /// <summary>
        /// 向窗体发送用户列表的消息
        /// </summary>
        /// <param name="str"></param>
        public void setUserListMessage(string str)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(setUserListMessage), new object[] { str });
                return;
            }
            userList.Items.Add(str);
        }
        /// <summary>
        /// 设置窗体记录的消息
        /// </summary>
        /// <param name="str"></param>
        public void setLogMessage(string str)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(setLogMessage), new object[] { str });
                return;
            }
            logTextBox.AppendText(String.Format("{0} \r\n", str));
        }
        /// <summary>
        /// 设置action的委托函数
        /// </summary>
        /// <param name="cdb"></param>
        void getConnectDataBase(ConnectDataBase cdb)
        {
            cdb.BoilerEventLog += new BoilerLogHandler(setLogMessage);
            cdb.BoilerEventUserList += new BoilerUserListHandler(setUserListMessage);
        }
        /// <summary>
        /// 窗体刷新按钮，清除过期的id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reflesh_Click(object sender, EventArgs e)
        {
             _wsServer.WebSocketServices.TryGetServiceHost("/ConnectDataBase",out wssk);
             this.userList.Items.Clear();
            foreach(string id in wssk.Sessions.IDs){
                this.userList.Items.Add(id);
            }
        }
        /// <summary>
        /// 选择窗体中需要发送消息的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.userList.SelectedItem == null) {
                return;
            }
            textBox_sendID.Text = this.userList.SelectedItem.ToString();
        }
    }
}
