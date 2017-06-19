using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;
using Model;
using Model.XmlModel;
using Attribute = Model.XmlModel.Attribute;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(MyCommonExceptionHandlingMethod);
            try
            {
                DialogResult result;
                using (var loginForm = new LoginForm())
                {
                    result = loginForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        var server = "localhost";
                        var port = 50000;
                        var ipHostInfo = Dns.GetHostEntry(server);
                        var ipAddress =
                            ipHostInfo.AddressList.FirstOrDefault(t => t.AddressFamily == AddressFamily.InterNetwork);
                        if (ipAddress == null)
                            throw new Exception("No IPv4 address for server");

                        var data = new Request()
                        {
                            Function = Functions.Authorization,
                            Attribute = new List<Attribute>()
                            {
                                new Attribute()
                                {
                                    Name = "login",
                                    Value = loginForm.textBox1.Text
                                },
                                new Attribute()
                                {
                                    Name = "password",
                                    Value = loginForm.textBox2.Text
                                }
                            }
                        };

                        var testClient = new TestClient(new TcpClient());
                        testClient.Connect(server, port);
                        var response = testClient.SendRequest(data);
                        if (response.Result.Code == 0)
                        {
                            Application.Run(new MainForm(testClient));
                        }
                        else
                        {
                            throw new Exception("Пользователь не существует");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void MyCommonExceptionHandlingMethod(object sender, ThreadExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
        }
    }

    public class TestClient : IDisposable
    {
        private TcpClient MyClient { get; }


        public TestClient(TcpClient tcpClient)
        {
            MyClient = tcpClient;
        }

        StreamWriter _writer;
        StreamReader _reader;

        public void Connect(string server, int port)
        {
            var ipHostInfo = Dns.GetHostEntry(server);
            var ipAddress = ipHostInfo.AddressList.FirstOrDefault(t => t.AddressFamily == AddressFamily.InterNetwork);
            if (ipAddress == null)
                throw new Exception("No IPv4 address for server");
            MyClient.Connect(ipAddress, port); // Connect
            var networkStream = MyClient.GetStream();
            _writer = new StreamWriter(networkStream);
            _reader = new StreamReader(networkStream);
            _writer.AutoFlush = true;
        }

        public async Task<Result> SendRequest<T>(T obj)
        {
            try
            {
                var data = Converters.ObjectToXml(obj);
                await _writer.WriteLineAsync(data);
                var response = await _reader.ReadLineAsync();
                var res = Converters.XmlToObject<Result>(response);
                return res;
            }
            catch (Exception)
            {
                return new Result() {Code = -1};
            }
        }

        public void Dispose()
        {
            MyClient.Close();
        }
    }
}