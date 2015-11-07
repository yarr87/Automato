using Automato.Model;
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
            //return new HomeState()
            //{
            //    Time = DateTime.Now,
            //    Users = new List<UserState>()
            //    {
            //        new UserState() { UserId = "jeff_user_1", IsHome = true }
            //    },
            //    Lights = new List<LightState>()
            //    {
            //        new LightState() { InternalName = "Z_test", State = "OFF" }
            //    }
            //};

            var homeState = new HomeState();

            homeState.Time = DateTime.Now;

            var lights = await new DeviceStore().GetDevices();
            homeState.Lights = lights.Select(l => new LightState() { InternalName = l.InternalName, State = l.State });

            var users = new UserStore().GetAll();
            homeState.Users = users.Select(u => new UserState() { UserId = u.Id, IsHome = u.IsHome });

            return homeState;
        }

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


