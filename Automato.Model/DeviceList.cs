using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Automato.Model
{
    [Serializable]
    [XmlRoot("DeviceList")]
    public class DeviceList
    {
        public List<Device> Devices { get; set; }
    }
}
