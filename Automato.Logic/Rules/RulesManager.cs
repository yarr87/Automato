using Automato.Logic.HomeStates;
using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Extensions;
using Automato.Model.HomeStates;
using Automato.Model.Messages;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules
{
    public class RulesManager
    {
        HomeStateManager _homeStateManager = new HomeStateManager();
        RulesEngine _rulesEngine = new RulesEngine();
        RuleRunner _ruleRunner = new RuleRunner();

        //List<Rule> _rules = new List<Rule>()
        //{
        //    new Rule() 
        //    {
        //        RuleDefinitions = new List<BaseRuleDefinition>()
        //        {
        //            new LightRule() 
        //            { 
        //                IsTriggered = true,
        //                LightState = new Model.HomeStates.LightState() { InternalName = "Z_switch_kitchen_island", State = "ON" }
        //            },
        //            //new UserRule()
        //            //{
        //            //    IsTriggered = false,
        //            //    UserState = new Model.HomeStates.UserState() { UserId = "jeff_user_1", IsHome = true }
        //            //}
        //        },
        //        Action = new RuleAction()
        //        {
        //            DeviceStates = new List<DeviceState>()
        //            {
        //                new DeviceState() { InternalName = "Z_switch_kitchen_sink", State = "ON" }
        //            }
        //        }
        //    },
        //    new Rule() 
        //    {
        //        RuleDefinitions = new List<BaseRuleDefinition>()
        //        {
        //            new LightRule() 
        //            { 
        //                IsTriggered = true,
        //                LightState = new Model.HomeStates.LightState() { InternalName = "Z_switch_kitchen_sink", State = "OFF" }
        //            },
        //            //new UserRule()
        //            //{
        //            //    IsTriggered = false,
        //            //    UserState = new Model.HomeStates.UserState() { UserId = "jeff_user_1", IsHome = true }
        //            //}
        //        },
        //        Action = new RuleAction()
        //        {
        //            DeviceStates = new List<DeviceState>()
        //            {
        //                new DeviceState() { InternalName = "Z_switch_kitchen_island", State = "OFF" }
        //            }
        //        }
        //    },
        //    new Rule() 
        //    {
        //        RuleDefinitions = new List<BaseRuleDefinition>()
        //        {
        //            new UserRule()
        //            {
        //                IsTriggered = true,
        //                UserState = new Model.HomeStates.UserState() { UserId = "users-1", IsHome = true }
        //            }
        //        },
        //        Action = new RuleAction()
        //        {
        //            DeviceStates = new List<DeviceState>()
        //            {
        //                new DeviceState() { InternalName = "Z_switch_kitchen_island", State = "ON" }
        //            }
        //        }
        //    }
        //};

        /// <summary>
        /// When a device state is updated, trigger any appropriate rules
        /// </summary>
        /// <param name="deviceStates"></param>
        /// <returns></returns>
        public async Task ProcessDeviceStateUpdates(IEnumerable<DeviceState> deviceStates)
        {
            var homeState = await _homeStateManager.GetCurrentHomeState();
            var rules = new RuleStore().GetAll();

            foreach (var deviceState in deviceStates)
            {
                var matchingRules = rules.Where(r => r.RuleDefinitions
                                                            .GetLightRules()
                                                            .Any(lr => lr.LightState.InternalName == deviceState.InternalName && lr.IsTriggered));
                    
                var updatedState = new Lazy<HomeState>(() => _homeStateManager.ApplyLightStateChange(homeState, deviceState));
                        
                foreach(var matchingRule in matchingRules)
                {
                    if (_rulesEngine.IsRuleActive(matchingRule, updatedState.Value))
                    {
                        await _ruleRunner.RunRule(matchingRule);
                    }
                }
            }
        }

        /// <summary>
        /// When a user's presence is updated, trigger any appropriate rules
        /// </summary>
        /// <param name="userUpdates"></param>
        /// <returns></returns>
        public async Task ProcessUserPresenceUpdates(IEnumerable<UserPresenceUpdate> userUpdates)
        {
            var homeState = await _homeStateManager.GetCurrentHomeState();

            var users = new UserStore().GetAll();
            var rules = new RuleStore().GetAll();

            foreach (var userUpdate in userUpdates)
            {
                var user = users.FirstOrDefault(u => u.DeviceMac == userUpdate.DeviceMac);

                if (user == null) continue;

                var matchingRules = rules.Where(r => r.RuleDefinitions.GetUserRules()
                                                            .Any(ur => ur.UserState.UserId == user.Id && ur.IsTriggered));

                var userState = new UserState() { UserId = user.Id, IsHome = userUpdate.IsHome };

                var updatedState = new Lazy<HomeState>(() => _homeStateManager.ApplyUserStateChange(homeState, userState));

                foreach (var matchingRule in matchingRules)
                {
                    if (_rulesEngine.IsRuleActive(matchingRule, updatedState.Value))
                    {
                        await _ruleRunner.RunRule(matchingRule);
                    }
                }
            }
        }
    }
}
