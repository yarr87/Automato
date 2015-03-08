//﻿using System;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using System.Text;

//using WebSocket4Net;
//using WebSocket4Net.Protocol;

//using AtmosphereWebSocket4Net.Protocol;

//namespace AtmosphereWebSocket4Net
//{
//    /// <summary>
//    /// WebSocket client wrapping which serializes/deserializes objects by JSON
//    /// </summary>
//    public partial class AtmosphereWebSocket : WebSocket
//    {
//        private static List<KeyValuePair<string, string>> customUrlQueryItems = new List<KeyValuePair<string, string>>()
//        {
//            new KeyValuePair<string, string>("X-Atmosphere-Framework", "2.0"),
//            new KeyValuePair<string, string>("X-Atmosphere-tracking-id", "0"),
//            new KeyValuePair<string, string>("X-Cache-Date", "0"),
//            new KeyValuePair<string, string>("X-atmo-protocol", "true"),
//            new KeyValuePair<string, string>("X-Atmosphere-Transport", "websocket"),
//            new KeyValuePair<string, string>("Content-Type", "application/json")
//        };

//        public AtmosphereWebSocket(string uri)
//            : base(uri, string.Empty, null, null, customUrlQueryItems, string.Empty, string.Empty, WebSocketVersion.Rfc6455)
//        {
//            SetProtocolProcessor(new AtmosphereProcessor());
//        }
//    }
//}