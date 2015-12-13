using Automato.Model;
using log4net;
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

        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                    Logger.DebugFormat("Received message: {0}", e.Data);

                    var updates = ProcessMessage(e.Data);

                    if (updates != null)
                    {
                        MessageReceived(updates);
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error("Error processing message", ex);
                }
            };

            _webSocket.OnError += (sender, e) =>
            {
                Logger.Error("Error on websocket connection", e.Exception);
            };

            _webSocket.OnClose += (sender, e) =>
            {
                Logger.Error("Websocket connection closed, reason: " + e.Reason);
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