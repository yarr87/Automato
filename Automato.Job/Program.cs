using Automato.Integration;
using Automato.Integration.Router;
using Automato.Logic.Rules;
using Automato.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Automato.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["OpenHab.WebSocketUrl"];

            //var updates = new List<DeviceState>()
            //{
            //    new DeviceState() { InternalName = "blah" },
            //    new DeviceState() { InternalName = "Z_test", State = "ON" }
            //};

            //var a = new RulesManager().ProcessDeviceStateUpdates(updates);
            //a.Wait();

            var userPresenceDetector = new UserPresenceDetector();

            userPresenceDetector.Start();
            

            var listener = new OpenHabListener();
            listener.Initialize(url);

            listener.MessageReceived = async (deviceStates) =>
            {
                await new ApiClient().SendStatusUpdates(deviceStates);
            };

            Console.ReadKey(true);
        }
    }


}
