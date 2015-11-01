using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Integration.Model
{
    /// <summary>
    /// Represents a device coming from the router's api
    /// </summary>
    public class NetworkDevice
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Mac { get; set; }

        [JsonProperty]
        public bool Status { get; set; }
    }
}
