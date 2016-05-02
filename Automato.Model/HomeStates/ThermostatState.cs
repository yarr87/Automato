using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.HomeStates
{
    /// <summary>
    /// State of a thermostat
    /// </summary>
    public class ThermostatState
    {
        public string ThermostatId { get; set; }

        public decimal? HeatSetPoint { get; set; }
        public decimal? CoolSetPoint { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }

        // 0 = Off, 1 = Heat, 2 = Cool
        public int Mode { get; set; }

        // 0 = Auto Low, 1 = On Low, 2 = Auto High, 3 = Auth High
        public int FanMode { get; set; }

        // 0 = Idle, 1 = Heat, 2 = Cool, 3 = Fan Only, 4 = Pending Heat, 5 = Pending Cool
        public int OperatingState { get; set; }

        // 0 = Idle, 1 = Running, 2 = Running High
        public int FanState { get; set; }

        public decimal? Battery { get; set; }

        public ThermostatState Copy()
        {
            return new ThermostatState()
            {
                ThermostatId = this.ThermostatId,
                HeatSetPoint = this.HeatSetPoint,
                CoolSetPoint = this.CoolSetPoint,
                Temperature = this.Temperature,
                Humidity = this.Humidity,
                Mode = this.Mode,
                FanMode = this.FanMode,
                OperatingState = this.OperatingState,
                FanState = this.FanState,
                Battery = this.Battery
            };
        }
    }
}
