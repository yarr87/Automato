using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    public enum DeviceType
    {
        /// <summary>
        /// Indicates this device is part of a group (ie, thermostat), and won't show in the device list
        /// </summary>
        SubItem,

        LightSwitch,
        Dimmer,
        Temperature

    }
}
