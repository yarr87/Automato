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
        string Name { get; set; }

        [JsonProperty]
        string Mac { get; set; }

        [JsonProperty]
        bool Status { get; set; }
    }
}
