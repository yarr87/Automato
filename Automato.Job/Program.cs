using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Automato.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            // AutoResetEvent closeEvent = new AutoResetEvent(false);

            //AtmosphereWebSocket webSocketClient = new AtmosphereWebSocket("ws://127.0.0.1:30222/notification/ksspush");
            //webSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(webSocketClient_MessageReceived);
            //webSocketClient.Open();

            //closeEvent.WaitOne();

            //          new KeyValuePair<string, string>("X-Atmosphere-Framework", "2.0"),
            //new KeyValuePair<string, string>("X-Atmosphere-tracking-id", "0"),
            //new KeyValuePair<string, string>("X-Cache-Date", "0"),
            //new KeyValuePair<string, string>("X-atmo-protocol", "true"),
            //new KeyValuePair<string, string>("X-Atmosphere-Transport", "websocket"),
            //new KeyValuePair<string, string>("Content-Type", "application/json")

            //var url = "ws://localhost:8080/rest/sitemaps/default/default?X-Atmosphere-tracking-id=abcd&X-Atmosphere-Framework=0.9&X-Atmosphere-Transport=websocket&X-Cache-Date=0&Accept=application%2Fjson";
            var url = "ws://localhost:8080/rest/sitemaps/default/default?X-Atmosphere-tracking-id=abcd&X-Atmosphere-Framework=0.9&X-Atmosphere-Transport=websocket&X-Cache-Date=0&Accept=application%2Fjson";

            using (var ws = new WebSocket(url)) //"ws://localhost:8080/rest/items/Z_switch1?X-Atmosphere-Transport=websocket&X-Atmosphere-tracking-id=1234&x-atmo-protocol=true&Accept=application/json"))

            //using (var ws = new WebSocket("ws://localhost:8080/rest/items?X-Atmosphere-Transport=websocket"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    Console.WriteLine("Message: " + e.Data);
                };
                ws.EnableRedirection = true;
                ws.Connect();
                //ws.Send("BALUS");
                Console.ReadKey(true);
            }

            //var task = Task.Run(async () =>
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            //        //client.DefaultRequestHeaders.Add("X-Atmosphere-Transport", "streaming");
            //        //client.DefaultRequestHeaders.Add("Accept", "application/json");

            //        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/rest/items/Z_switch1?X-Atmosphere-Transport=streaming&X-Atmosphere-tracking-id=1234&x-atmo-protocol=true&Accept=application/json");
            //        using (var response = await client.SendAsync(
            //            request,
            //            HttpCompletionOption.ResponseHeadersRead))
            //        {
            //            using (var body = await response.Content.ReadAsStreamAsync())
            //            using (var reader = new StreamReader(body))
            //                while (!reader.EndOfStream)
            //                    Console.WriteLine(reader.ReadLine());
            //        }
            //    }

            //    //using (HttpClient httpClient = new HttpClient())
            //    //{

            //    //    httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            //    //    httpClient.DefaultRequestHeaders.Add("X-Atmosphere-Transport", "streaming");
            //    //    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");


            //    //    var requestUri = "http://localhost:8080/rest/items/Z_switch1";
            //    //    var stream = await httpClient.GetStreamAsync(requestUri);

            //    //    using (var reader = new StreamReader(stream))
            //    //    {

            //    //        while (!reader.EndOfStream)
            //    //        {

            //    //            //We are ready to read the stream
            //    //            var currentLine = reader.ReadLine();

            //    //            Console.WriteLine(currentLine);
            //    //        }
            //    //    }
            //    //}
            //});
            //task.Wait();
        }
    }
}
