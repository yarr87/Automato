using Automato.Integration;
using Automato.Integration.Router;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Automato.Job
{
    class Program
    {
        static void Main(string[] args)
        {
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

            //foreach (var rule in _rules)
            //{
            //    //new RuleStore().Save(rule);
            //}

            //var saved = new RuleStore().GetAll();

            //var a = saved;

            //return;


            var url = ConfigurationManager.AppSettings["OpenHab.WebSocketUrl"];

            //var updates = new List<DeviceState>()
            //{
            //    new DeviceState() { InternalName = "blah" },
            //    new DeviceState() { InternalName = "Z_test", State = "ON" }
            //};

            //var a = new RulesManager().ProcessDeviceStateUpdates(updates);
            //a.Wait();

            var userPresenceDetector = new UserPresenceDetector();

            userPresenceDetector.Start();
            

            var listener = new OpenHabListener();
            listener.Initialize(url);

            listener.MessageReceived = async (deviceStates) =>
            {
                await new ApiClient().SendStatusUpdates(deviceStates);
            };

            Console.ReadKey(true);
        }
    }


}
