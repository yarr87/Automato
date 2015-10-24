using Automato.Integration;
using Automato.Integration.Router;
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

            try
            {
                using (var router = new RouterApi())
                {
                    // Get the list of devices on the network.  When compared with the previous state, this will tell us if someone just came home.
                    // How to handle this?
                    // 1) Send these as-is to the api and let it handle everything.  If someone just came home, trigger any associated jobs
                    // 2) Send these to the api to be stored in a table.  All events that are time-based will check state every minute or so and trigger
                    //    off of that.  Only issue here: how will it know if someone just came home?  Would have to compare between last state and current state.
                    // 3) Do everything here (trigger someone just came home, etc)

                    // I think there should be some sort of "just came home" trigger that is fired from the api, which can immediately run some rules.  Need to be careful
                    // that it doesn't take too long so the api call to update presence isn't slowed down.  Normal time-based rules can run periodically and check state, but
                    // those that depend on a state changing immediately should be triggered right then.  I wouldn't want someone to come home, but have to wait a minute or
                    // however long for the actual rule to trigger, especially if we are turning lights on for them.
                    var devices = router.GetNetworkDevices().Result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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
