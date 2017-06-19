using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Model.XmlModel;
using Attribute = Model.XmlModel.Attribute;

namespace AsyncServer
{
    static class ServiceProgram
    {
        static void Main(string[] args)
        {
            try
            {
                const int port = 50000;
                IAsyncService service = new AsyncService(port);
                service.Run();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}