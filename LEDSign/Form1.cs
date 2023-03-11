using System.Diagnostics;
using System.Net;

namespace ledsign
{
    public partial class Form1 : Form
    {

        private string _twitchOAuth = null;
        private string _twitchChannelName = null;
        private string _twitchChatCommand = null;
        private string _webServerIP = null;

        private Task chatBotTask;

        public Form1()
        {
            InitializeComponent();
            ReadConfiguration();
        }

        public void ReadConfiguration()
        {
            try
            {
                TextReader tr = new StreamReader("ledsign.conf");
                
                this._twitchOAuth = tr.ReadLine();
                this._twitchChannelName = tr.ReadLine();
                this._twitchChatCommand = tr.ReadLine();
                this._webServerIP = tr.ReadLine();

                this.twitchChannelName.Text = this._twitchChannelName;
                this.twitchChatCommand.Text = this._twitchChatCommand;
                this.webServerIP.Text = this._webServerIP;

                tr.Close();
            }
            catch (Exception err)
            {

            }
        }

        public void WriteConfiguration()
        {
            TextWriter tw = new StreamWriter("ledsign.conf");

            // write lines of text to the file
            tw.WriteLine(this._twitchOAuth);
            tw.WriteLine(this.twitchChannelName.Text);
            tw.WriteLine(this.twitchChatCommand.Text);
            tw.WriteLine(this.webServerIP.Text);

            // close the stream     
            tw.Close();
            LogText(this.richTextBox1, "*", "configuration saved", "system");
            ReadConfiguration();
        }

        public static void LogText(RichTextBox box, string author, string text, string type = "")
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            if (type == "system") {
                box.SelectionColor = Color.Blue;
            }
            else if (type == "command")
            {
                box.SelectionColor = Color.Red;
            }
            else
            {
                box.SelectionColor = Color.Black;
            }

            if (author != "*")
            {
                author = author + ":";
            }

            box.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + author + " " + text + "\n");
            box.SelectionColor = box.ForeColor;
            box.SelectionStart = box.Text.Length;
            // scroll it automatically
            box.ScrollToCaret();
        
     }

        public static async Task RunTwitchBot(Form1 form1)
        {
            string password = form1._twitchOAuth;
            string botUsername = form1._twitchChannelName;

            var twitchBot = new TwitchBot(botUsername, password);
            _ = twitchBot.Start();

            // We could .SafeFireAndForget() these two calls if we want to
            await twitchBot.JoinChannel(form1._twitchChannelName);
            await twitchBot.SendMessage(form1._twitchChannelName, "Connected to chat!");

            twitchBot.OnMessage += async (sender, twitchChatMessage) =>
            {
                string command = "";
                if (string.IsNullOrWhiteSpace(form1.twitchChatCommand.Text.ToString()))
                {
                    command = "robezonetv";
                }

                command = form1.twitchChatCommand.Text.ToString();

                if (twitchChatMessage.Message.StartsWith("!" + command + " "))
                {
                    string[] words = twitchChatMessage.Message.Split(' ');
                    if (words[1] != null)
                    {
                        int key = 3;
                        bool result = int.TryParse(words[1], out key);
                        if (result == true)
                        {

                            if (key == 2 && form1.allowOff.Checked == false)
                            {
                                key = 3;
                            }

                            LogText(form1.richTextBox1, "*", "value in key[" + key.ToString() + "]", "system");

                            try
                            {
                                var url = "http://" + form1._webServerIP + "/led?key=3";
                                var request = WebRequest.Create(url);
                                request.Method = "GET";
                                request.GetResponse();

                                url = "http://" + form1._webServerIP + "/led?key=" + key.ToString();
                                request = WebRequest.Create(url);
                                request.Method = "GET";
                                request.GetResponse();

                                LogText(form1.richTextBox1, twitchChatMessage.Sender, twitchChatMessage.Message, "command");
                            }
                            catch (Exception err)
                            {
                                LogText(form1.richTextBox1, "*", "can't connect to webserver", "system");
                            }
                        }
                        else
                        {
                            LogText(form1.richTextBox1, twitchChatMessage.Sender, twitchChatMessage.Message);
                        }
                    }
                    else
                    {
                        LogText(form1.richTextBox1, twitchChatMessage.Sender, twitchChatMessage.Message);
                    }
                }
                else
                {
                    LogText(form1.richTextBox1, twitchChatMessage.Sender, twitchChatMessage.Message);
                }
            };

            await Task.Delay(-1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WriteConfiguration();
            if (this._twitchOAuth == null)
            {
                LogText(this.richTextBox1, "*", "First use 'Twitch Auth' button ...", "system");
                return;
            }

            if (this._twitchChannelName == null || this._twitchChannelName.Length < 1)
            {
                LogText(this.richTextBox1, "*", "Write yout channel name to 'Connect to channel' ...", "system");
                return;
            }

            if (this._twitchChatCommand == null || this._twitchChatCommand.Length < 1)
            {
                LogText(this.richTextBox1, "*", "Write your command in your chat which can control me ...", "system");
                return;
            }

            if (this._webServerIP == null || this._webServerIP.Length < 1)
            {
                LogText(this.richTextBox1, "*", "Setup webserver where ESP8266 is listening for commands ...", "system");
                return;
            }

            if (this.chatBotTask == null)
            {
                this.twitchAuthButton.Enabled = false;
                this.chatLoginButton.Enabled = false;
                LogText(this.richTextBox1, "*", "Starting connect to Twitch chat...", "system");
                this.chatBotTask = RunTwitchBot(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WriteConfiguration();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.twitchAuthButton.Enabled = false;
            this.chatLoginButton.Enabled = false;
            LogText(this.richTextBox1, "*", "starting local webserver for receive OAuth token", "system");
            var httpServer = new HttpServer();
            httpServer.Start();
            string url = "https://id.twitch.tv/oauth2/authorize?client_id=9k1yeh15k4ida3c3xeqrpmtm8tvzkk&redirect_uri=http://localhost:22222&response_type=token&scope=chat:edit%20chat:read&force_verify=true";
            Process.Start("rundll32", "url.dll,FileProtocolHandler " + url);
            while (true)
            {
                this._twitchOAuth = httpServer.getTwitchToken();
                Thread.Sleep(1000);
                if (this._twitchOAuth != "NIC") {
                    break;
                }
            }
            LogText(this.richTextBox1, "*", "OAuth token received and closing webserver", "system");
            Thread.Sleep(1000);
            httpServer.Stop();
            WriteConfiguration();
            this.twitchAuthButton.Enabled = true;
            this.chatLoginButton.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteConfiguration();
        }
    }
}
