using Automato.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Job
{
    public class ApiClient
    {
        public async Task SendStatusUpdates(IEnumerable<DeviceStateUpdate> updates)
        {
            var apiBaseUrl = "http://192.168.0.2:49310/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                var content = JsonConvert.SerializeObject(updates);

                try
                {
                    await client.PostAsync("/api/devicestates", new StringContent(content, Encoding.UTF8, "application/json"));
                }
                catch (Exception ex)
                {
                    var x = ex;
                }
            }
        }
    }
}
