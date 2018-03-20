namespace WebSocketWinForm
{
    partial class WebSocketDemo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.userList = new System.Windows.Forms.ListBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.textBox_sendID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.btn_reflesh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // userList
            // 
            this.userList.FormattingEnabled = true;
            this.userList.ItemHeight = 12;
            this.userList.Location = new System.Drawing.Point(29, 23);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(448, 148);
            this.userList.TabIndex = 0;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(526, 113);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(168, 23);
            this.btn_send.TabIndex = 1;
            this.btn_send.Text = "发送";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // textBox_sendID
            // 
            this.textBox_sendID.Location = new System.Drawing.Point(526, 50);
            this.textBox_sendID.Name = "textBox_sendID";
            this.textBox_sendID.Size = new System.Drawing.Size(168, 21);
            this.textBox_sendID.TabIndex = 2;
            this.textBox_sendID.Text = "输入发送的sessionID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(583, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "用户ID";
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(526, 142);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(168, 23);
            this.btn_close.TabIndex = 4;
            this.btn_close.Text = "结束";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(29, 209);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(665, 161);
            this.logTextBox.TabIndex = 5;
            this.logTextBox.Text = "";
            // 
            // btn_reflesh
            // 
            this.btn_reflesh.Location = new System.Drawing.Point(29, 180);
            this.btn_reflesh.Name = "btn_reflesh";
            this.btn_reflesh.Size = new System.Drawing.Size(111, 23);
            this.btn_reflesh.TabIndex = 6;
            this.btn_reflesh.Text = "刷新用户列表";
            this.btn_reflesh.UseVisualStyleBackColor = true;
            this.btn_reflesh.Click += new System.EventHandler(this.btn_reflesh_Click);
            // 
            // WebSocketDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 396);
            this.Controls.Add(this.btn_reflesh);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_sendID);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.userList);
            this.Name = "WebSocketDemo";
            this.Text = "WebSocketDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox textBox_sendID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_close;
        public System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Button btn_reflesh;
    }
}

