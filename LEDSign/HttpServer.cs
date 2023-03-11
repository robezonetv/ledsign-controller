using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ledsign
{
    internal class HttpServer
    {
        public string twitchOauthToken = "NIC";
        public int Port = 22222;

        private HttpListener _listener;

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://127.0.0.1:" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public string getTwitchToken()
        {
            return this.twitchOauthToken;
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            try
            {
                if (_listener.IsListening)
                {
                    var context = _listener.EndGetContext(result);
                    var request = context.Request;

                    // Ulozit si
                    if (request.QueryString["access_token"] != null)
                    {
                        this.twitchOauthToken = "oauth:" + request.QueryString["access_token"];
                    }

                    // do something with the request
                    var response = context.Response;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.ContentType = "text/html; charset=UTF-8";
                    if (request.QueryString["access_token"] == null)
                    {
                        response.OutputStream.Write(Encoding.ASCII.GetBytes("You should be redirected ... <script>window.location.replace('http://localhost:22222/?'.concat(window.location.hash.split('#')[1]));</script>"));
                    }
                    else
                    {
                        response.OutputStream.Write(Encoding.ASCII.GetBytes("Token received! You can close this page."));
                    }
                    response.OutputStream.Close();

                    Receive();
                }
            }
            catch (Exception err)
            {
                // Your logic threw an exception. handle it accordinhly
            }
        }
    }
}
