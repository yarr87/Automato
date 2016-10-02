using Automato.Integration;
using Automato.Logic.Stores;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    /// <summary>
    /// Handles setting thermostat temperature - updates both heat and cool setpoints so we don't have to worry about
    /// what mode the thermostat is in
    /// </summary>
    public class TemperatureHandler
    {
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task SetTemperature(string thermostatId, string temperature)
        {
            var thermostat = new ThermostatStore().GetById(thermostatId);

            if (thermostat != null)
            {
                var openhab = new OpenHabRestService();

                // I decided that switching mode (cool/heat/off) would be a manual step that I do a couple times a year.  So, when setting thermostat
                // temp just always keep heat and cool setpoints in sync.  The mode that turns on will be set manually.

                // NOTE: originally this did heat then cool, and it would update heat but not cool.  Worked fine downstairs.  Switching them
                // fixed it.  However, I'm not sure if that was because downstairs was in heat/off mode and upstairs was in cool.  Will have to
                // see if next winter it breaks heat set point when in heat mode.

                // 10/2: this worked all summer in A/C mode, but when I switched upstairs to heat it stopped working.  I'm guessing it can't handle getting
                // two commands in a row so close together.  Solution would be (1) add a delay between them, or (2) include heat/cool type in the request.  For
                // now just switching them back so heat will work.
                // Would need to add a heat/cool switch
                // 1) On the main thermostat UI
                // 2) In the TemperatureRuleAction object, and the associated rule action in the frontend.
                await openhab.SendCommand(thermostat.HeatSetPoint.InternalName, temperature);
                await openhab.SendCommand(thermostat.CoolSetPoint.InternalName, temperature);
            }
            else
            {
                Logger.ErrorFormat("Could not find thermostat with id {0}", thermostatId);
            }
        }
    }
}
