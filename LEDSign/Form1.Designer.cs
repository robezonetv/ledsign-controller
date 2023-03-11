namespace ledsign
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.twitchChatCommand = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.webServerIP = new System.Windows.Forms.TextBox();
            this.twitchAuthButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.clearChatButton = new System.Windows.Forms.Button();
            this.saveConfigButton = new System.Windows.Forms.Button();
            this.chatLoginButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.twitchChannelName = new System.Windows.Forms.TextBox();
            this.allowOff = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // twitchChatCommand
            // 
            this.twitchChatCommand.Location = new System.Drawing.Point(14, 80);
            this.twitchChatCommand.Name = "twitchChatCommand";
            this.twitchChatCommand.PlaceholderText = "party";
            this.twitchChatCommand.Size = new System.Drawing.Size(155, 23);
            this.twitchChatCommand.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Adresa ESP8266";
            // 
            // webServerIP
            // 
            this.webServerIP.Location = new System.Drawing.Point(14, 134);
            this.webServerIP.Name = "webServerIP";
            this.webServerIP.PlaceholderText = "192.168.0.100";
            this.webServerIP.Size = new System.Drawing.Size(155, 23);
            this.webServerIP.TabIndex = 102;
            // 
            // twitchAuthButton
            // 
            this.twitchAuthButton.Location = new System.Drawing.Point(343, 12);
            this.twitchAuthButton.Name = "twitchAuthButton";
            this.twitchAuthButton.Size = new System.Drawing.Size(112, 29);
            this.twitchAuthButton.TabIndex = 104;
            this.twitchAuthButton.Text = "Twitch Auth";
            this.twitchAuthButton.UseVisualStyleBackColor = true;
            this.twitchAuthButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Command v chatu (bez !)";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(14, 217);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(440, 177);
            this.richTextBox1.TabIndex = 107;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Chat log";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // clearChatButton
            // 
            this.clearChatButton.Location = new System.Drawing.Point(379, 400);
            this.clearChatButton.Name = "clearChatButton";
            this.clearChatButton.Size = new System.Drawing.Size(75, 23);
            this.clearChatButton.TabIndex = 108;
            this.clearChatButton.Text = "Clear chat";
            this.clearChatButton.UseVisualStyleBackColor = true;
            this.clearChatButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // saveConfigButton
            // 
            this.saveConfigButton.Location = new System.Drawing.Point(343, 83);
            this.saveConfigButton.Name = "saveConfigButton";
            this.saveConfigButton.Size = new System.Drawing.Size(112, 30);
            this.saveConfigButton.TabIndex = 106;
            this.saveConfigButton.Text = "Save config";
            this.saveConfigButton.UseVisualStyleBackColor = true;
            this.saveConfigButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // chatLoginButton
            // 
            this.chatLoginButton.Location = new System.Drawing.Point(343, 47);
            this.chatLoginButton.Name = "chatLoginButton";
            this.chatLoginButton.Size = new System.Drawing.Size(112, 30);
            this.chatLoginButton.TabIndex = 105;
            this.chatLoginButton.Text = "Chat Login";
            this.chatLoginButton.UseVisualStyleBackColor = true;
            this.chatLoginButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Connect to channel";
            // 
            // twitchChannelName
            // 
            this.twitchChannelName.Location = new System.Drawing.Point(14, 30);
            this.twitchChannelName.Name = "twitchChannelName";
            this.twitchChannelName.PlaceholderText = "robezonetv";
            this.twitchChannelName.Size = new System.Drawing.Size(155, 23);
            this.twitchChannelName.TabIndex = 100;
            // 
            // allowOff
            // 
            this.allowOff.AutoSize = true;
            this.allowOff.Location = new System.Drawing.Point(14, 173);
            this.allowOff.Name = "allowOff";
            this.allowOff.Size = new System.Drawing.Size(170, 19);
            this.allowOff.TabIndex = 103;
            this.allowOff.Text = "Allow to poweroff LED Sign";
            this.allowOff.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 434);
            this.Controls.Add(this.allowOff);
            this.Controls.Add(this.twitchChannelName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chatLoginButton);
            this.Controls.Add(this.saveConfigButton);
            this.Controls.Add(this.clearChatButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.twitchChatCommand);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.twitchAuthButton);
            this.Controls.Add(this.webServerIP);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "LEDSign by robezonetv";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox webServerIP;
        private Button twitchAuthButton;
        private Label label2;
        private TextBox twitchChatCommand;
        private RichTextBox richTextBox1;
        private Label label3;
        private Button clearChatButton;
        private Button saveConfigButton;
        private Button chatLoginButton;
        private Label label4;
        private TextBox twitchChannelName;
        private CheckBox allowOff;
    }
}