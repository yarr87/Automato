using Automato.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebSocketSharp;

namespace Automato.Integration
{
    /// <summary>
    /// Listens for device updates from OpenHab via websocket
    /// </summary>
    public class OpenHabListener : IDisposable
    {
        public Action<IEnumerable<DeviceState>> MessageReceived { get; set; }

        private WebSocket _webSocket;

        public void Initialize(string webSocketUrl)
        {
            // This is listening to the entire sitemap, but updates only include the item that changed!

            if (_webSocket != null)
            {
                _webSocket.Close();
            }

            _webSocket = new WebSocket(webSocketUrl);

            _webSocket.OnMessage += (sender, e) =>
            {
                try
                {
                    Console.WriteLine("Message: " + e.Data);

                    var updates = ProcessMessage(e.Data);

                    if (updates != null)
                    {
                        MessageReceived(updates);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };

            _webSocket.OnError += (sender, e) =>
            {
                Console.WriteLine("Error on websocket connection: " + e.Message);
            };

            _webSocket.Connect();
        }

        private IEnumerable<DeviceState> ProcessMessage(string message)
        {
            using (var reader = new StringReader(message))
            {
                var xml = XDocument.Load(reader);

                var widgets = xml.Element("widgets");

                if (widgets != null)
                {
                    var updates = (from widget in widgets.Elements("widget")
                                   from item in widget.Elements("item")
                                   select new DeviceState()
                                   {
                                       InternalName = item.Element("name").Value,
                                       State = item.Element("state").Value
                                   }).ToList();

                    return updates;
                }
            }

            return null;
        }

        public void Dispose()
        {
            if (_webSocket != null)
            {
                _webSocket.Close();
            }
        }
    }
}