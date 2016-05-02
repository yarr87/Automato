using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Extensions;
using Automato.Model.HomeStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.HomeStates
{
    public class HomeStateManager
    {
        /// <summary>
        /// Returns information about the current state of everything in the house
        /// </summary>
        /// <returns></returns>
        public async Task<HomeState> GetCurrentHomeState()
        {
            var homeState = new HomeState();

            homeState.Time = DateTime.Now;

            var lights = await new DeviceStore().GetDevices();
            homeState.Lights = lights.Where(l => l.Type == DeviceType.LightSwitch || l.Type == DeviceType.Dimmer)
                                     .Select(l => new LightState() { InternalName = l.InternalName, State = l.State });

            var users = new UserStore().GetAll();
            homeState.Users = users.Select(u => new UserState() { UserId = u.Id, IsHome = u.IsHome });

            var thermostats = await new ThermostatStore().GetAllWithState();
            homeState.Thermostats = thermostats.Select(t => new ThermostatState()
                {
                    ThermostatId = t.Id,
                    HeatSetPoint = t.HeatSetPoint.State.ToDecimal(),
                    CoolSetPoint = t.CoolSetPoint.State.ToDecimal(),
                    Temperature = t.Temperature.State.ToDecimal(),
                    Humidity = t.Humidity.State.ToDecimal(),
                    Mode = t.Mode.State.ToInt(),
                    FanMode = t.FanMode.State.ToInt(),
                    OperatingState = t.OperatingState.State.ToInt(),
                    FanState = t.FanState.State.ToInt(),
                    Battery = t.Battery.State.ToDecimal()
                });

            return homeState;
        }

        /// <summary>
        /// Applies the given light state change to the given home state, and returns the newly calculated home state.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="lightState"></param>
        /// <returns></returns>
        public HomeState ApplyLightStateChange(HomeState currentState, DeviceState lightState)
        {
            var newState = currentState.Copy();

            var updatedLight = newState.Lights.FirstOrDefault(l => l.InternalName == lightState.InternalName);

            if (updatedLight != null)
            {
                updatedLight.State = lightState.State;
            }

            return newState;
        }

        /// <summary>
        /// Applies the given user state change to the given home state, and returns the newly calculated home state.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="userState"></param>
        /// <returns></returns>
        public HomeState ApplyUserStateChange(HomeState currentState, UserState userState)
        {
            var newState = currentState.Copy();

            var updatedUser = newState.Users.FirstOrDefault(u => u.UserId == userState.UserId);

            if (updatedUser != null)
            {
                updatedUser.IsHome = userState.IsHome;
            }

            return newState;
        }
    }
}


