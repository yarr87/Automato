using Automato.Integration;
using Automato.Integration.Router;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
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

            var userPresenceDetector = new UserPresenceDetector();
            userPresenceDetector.Start();

            new RuleDetector().Start();

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
