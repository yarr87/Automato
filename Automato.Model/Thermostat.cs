using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    public class Thermostat : ICopyable<Thermostat>
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Friendly Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start of the internal name for each sub device.
        /// Ex: if this is HVAC_Down, then HeatSetPoint will be HVAC_Down_HeatSetPoint
        /// </summary>
        public string InternalNamePrefix { get; set; }

        public Device HeatSetPoint { get; set; }
        public Device CoolSetPoint { get; set; }
        public Device Temperature { get; set; }
        public Device Humidity { get; set; }

        // 0 = Off, 1 = Heat, 2 = Cool
        public Device Mode { get; set; }

        // 0 = Auto Low, 1 = On Low, 2 = Auto High, 3 = Auth High
        public Device FanMode { get; set; }

        // 0 = Idle, 1 = Heat, 2 = Cool, 3 = Fan Only, 4 = Pending Heat, 5 = Pending Cool
        public Device OperatingState { get; set; }

        // 0 = Idle, 1 = Running, 2 = Running High
        public Device FanState { get; set; }

        public Device Battery { get; set; }

        public void CopyTo(Thermostat destination)
        {
            destination.Name = this.Name;
            destination.InternalNamePrefix = this.InternalNamePrefix;
            destination.HeatSetPoint = this.HeatSetPoint;
            destination.CoolSetPoint = this.CoolSetPoint;
            destination.Temperature = this.Temperature;
            destination.Humidity = this.Humidity;
            destination.Mode = this.Mode;
            destination.FanMode = this.FanMode;
            destination.OperatingState = this.OperatingState;
            destination.FanState = this.FanState;
            destination.Battery = this.Battery;
        }
    }
}
