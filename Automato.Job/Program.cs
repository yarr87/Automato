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
            ////using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            ////{
            //    try
            //    {
            //        var smtp = new SmtpClient
            //        {
            //            Host = "smtp.gmail.com",
            //            Port = 587,
            //            EnableSsl = true,
            //            DeliveryMethod = SmtpDeliveryMethod.Network,
            //            Credentials = new NetworkCredential("rosenberg19fitz@gmail.com", "Th3Hous419*Fi84#"),
            //            Timeout = 3000
            //        };

            //        //smtp.UseDefaultCredentials = false;

            //        var msg = new MailMessage("rosenberg19fitz@gmail.com", "6173127854@text.republicwireless.com");
            //        msg.Body = "testing";

            //        smtp.Send(msg);
            //    }
            //    catch (Exception ex)
            //    {
            //        var x = ex.Message;
            //    }
            ////}

            //return;

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
