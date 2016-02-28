using Automato.Model;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
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
    /// <summary>
    /// Interacts with sonos via a node http service
    /// </summary>
    public class SonosHttpService
    {
        private string SonosUrl = ConfigurationManager.AppSettings["Sonos.Url"];
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private struct SonosCommands
        {
            public const string GetFavorites = "favorites";
            public const string QueueFavorite = "favorite";
            public const string Next = "next";
            public const string Pause = "pause";
            public const string Play = "play";
        }

        public async Task<HttpResponseMessage> SendCommand(string name, string command, string parameter = "")
        {
            var url = string.Format("{0}/{1}/{2}", name, command, parameter);

            try
            {
                using (var client = GetClient())
                {
                    Logger.DebugFormat("Sending sonos command to {0}", url);

                    var response = await client.GetAsync(url);

                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error sending sonos command to " + url, ex);
                // TODO: what to do if this fails?  for now nothing since the connection is off a lot anyways

                return null;
            }
        }
        
        /// <summary>
        /// Move to the next song in the queue
        /// </summary>
        /// <param name="sonos"></param>
        /// <returns></returns>
        public async Task Next(string sonos)
        {
            await SendCommand(sonos, SonosCommands.Next);
        }

        /// <summary>
        /// Play the Favorite with the given name on the given sonos
        /// </summary>
        /// <param name="sonos"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task PlayFavorite(string sonos, string favorite)
        {
            await SendCommand(sonos, SonosCommands.QueueFavorite, favorite);
            await SendCommand(sonos, SonosCommands.Play);
        }

        /// <summary>
        /// Play the currently queued song
        /// </summary>
        /// <param name="sonos"></param>
        /// <returns></returns>
        public async Task Play(string sonos)
        {
            await SendCommand(sonos, SonosCommands.Play);
        }

        /// <summary>
        /// Pause the current song
        /// </summary>
        /// <param name="sonos"></param>
        /// <returns></returns>
        public async Task Pause(string sonos)
        {
            await SendCommand(sonos, SonosCommands.Pause);
        }

        public async Task<IEnumerable<string>> GetFavorites(string sonos)
        {
            var response = await SendCommand(sonos, SonosCommands.GetFavorites);

            if (response == null)
            {
                return new List<string>();
            }

            var stream = response.Content as StreamContent;
            var bytes = await stream.ReadAsByteArrayAsync();

            var content = Encoding.UTF8.GetString(bytes);

            // Errors just return like this, not an actual http error
            if (content.StartsWith("{\"error\""))
            {
                return new List<string>();
            }

            var favorites = JsonConvert.DeserializeObject<IEnumerable<string>>(content);

            return favorites;
        }

        private HttpClient GetClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(SonosUrl)
            };
        }
    }
}
