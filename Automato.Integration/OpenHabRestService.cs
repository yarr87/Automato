using Automato.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Automato.Integration
{
    public class OpenHabRestService
    {
        private string OpenHabUrl = ConfigurationManager.AppSettings["OpenHab.Url"];

        public async Task SendCommand(string deviceName, string command)
        {
            try
            {
                using (var client = GetClient())
                {
                    await client.PostAsync("rest/items/" + deviceName, new StringContent(command));
                }
            }
            catch (Exception ex)
            {
                // TODO: what to do if this fails?  for now nothing since the connection is off a lot anyways
            }
        }

        public async Task<IEnumerable<DeviceState>> GetDeviceStates()
        {
            try
            {
                using (var client = GetClient())
                {
                    var stream = await client.GetStreamAsync("rest/items/");

                    using (var reader = new StreamReader(stream))
                    {
                        var xml = XDocument.Load(reader);

                        var items = xml.Element("items");

                        if (items != null)
                        {
                            var updates = (from item in items.Elements("item")
                                           select new DeviceState()
                                           {
                                               InternalName = item.Element("name").Value,
                                               State = item.Element("state").Value
                                           }).ToList();


                            var x = updates;

                            return updates;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // nothing, openhab isn't on
            }

            return new List<DeviceState>();
        }

        private HttpClient GetClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(OpenHabUrl)
            };
        }
    }
}
