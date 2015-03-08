//﻿using System;
//using System.Collections.Specialized;
//using System.IO;
//using System.Collections.Generic;
//using System.Text;
//using System.Security.Cryptography;

//using WebSocket4Net;
//using WebSocket4Net.Protocol;

//namespace AtmosphereWebSocket4Net.Protocol
//{
//    /// <summary>
//    /// https://github.com/Atmosphere/atmosphere/wiki/Understanding-the-Atmosphere-Protocol
//    /// </summary>
//    class AtmosphereProcessor : Rfc6455Processor
//    {
//        private string atmosphereTrackingId = "0";

//        public override bool VerifyHandshake(WebSocket websocket, WebSocketCommandInfo handshakeInfo, out string description)
//        {
//            if (base.VerifyHandshake(websocket, handshakeInfo, out description))
//            {
//                atmosphereTrackingId = websocket.Items.GetValue("X-Atmosphere-tracking-id", string.Empty);
//                KeyValuePair<string, string> newId = new KeyValuePair<string, string>("X-Atmosphere-tracking-id", atmosphereTrackingId);
//                websocket.CustomUrlQueryItems.RemoveAll(x => x.Key.Equals(newId.Key));
//                websocket.CustomUrlQueryItems.Add(newId);
//                return true;
//            }

//            return false;
//        }
//    }
//}