using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public class ThermostatStore : BaseStore<Thermostat>
    {
        protected override Func<Thermostat, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }

        public override void OnBeforeSave(Thermostat entity)
        {
            base.OnBeforeSave(entity);

            entity.HeatSetPoint = SetSubDeviceId(entity.HeatSetPoint, entity.InternalNamePrefix, "HeatSetPoint");
            entity.CoolSetPoint = SetSubDeviceId(entity.CoolSetPoint, entity.InternalNamePrefix, "CoolSetPoint");
            entity.Temperature = SetSubDeviceId(entity.Temperature, entity.InternalNamePrefix, "Temperature");
            entity.Humidity = SetSubDeviceId(entity.Humidity, entity.InternalNamePrefix, "Humidity");
            entity.Mode = SetSubDeviceId(entity.Mode, entity.InternalNamePrefix, "Mode");
            entity.FanMode = SetSubDeviceId(entity.FanMode, entity.InternalNamePrefix, "Fan_Mode");
            entity.OperatingState = SetSubDeviceId(entity.OperatingState, entity.InternalNamePrefix, "Operating_State");
            entity.FanState = SetSubDeviceId(entity.FanState, entity.InternalNamePrefix, "Fan_State");
            entity.Battery = SetSubDeviceId(entity.Battery, entity.InternalNamePrefix, "Battery");
        }

        private Device SetSubDeviceId(Device device, string internalNamePrefix, string suffix)
        {
            if (device == null)
            {
                device = new Device();
            }

            device.InternalName = string.Format("{0}_{1}", internalNamePrefix, suffix);

            return device;
        }
    }
}
