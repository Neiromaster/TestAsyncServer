using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncServer;

namespace TestWindowsService
{
    public partial class WebService : ServiceBase
    {
        public WebService()
        {
            InitializeComponent();
        }

        private Thread _serviceThread;
        private IAsyncService _asyncService;
        protected override void OnStart(string[] args)
        {
            _asyncService = new AsyncService(50000);
            _serviceThread = new Thread(_asyncService.Run);
            _serviceThread.Start();
        }

        protected override void OnStop()
        {
            _asyncService.Stop();
            Thread.Sleep(1000);
        }
    }
}
