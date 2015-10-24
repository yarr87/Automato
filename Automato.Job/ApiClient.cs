using Automato.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Job
{
    public class ApiClient
    {
        /// <summary>
        /// Send a list of device statuses to the api.  Usually this will be one at a time in response to an openhab
        /// server push after a switch is updated.
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        public async Task SendStatusUpdates(IEnumerable<DeviceState> updates)
        {
            var apiBaseUrl = ConfigurationManager.AppSettings["Api.Url"];// "http://localhost:49310/";// "http://192.168.0.2:49310/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var content = JsonConvert.SerializeObject(updates);

                try
                {
                    Console.WriteLine("Posting content to " + client.BaseAddress + "api/devicestates");
                    Console.WriteLine(content);
                    await client.PostAsync("api/devicestates", new StringContent(content, Encoding.UTF8, "application/json"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
