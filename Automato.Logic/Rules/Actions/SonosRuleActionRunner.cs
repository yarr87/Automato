﻿using Automato.Integration;
using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    /// <summary>
    /// Execute a rule action that sends a sonos command
    /// </summary>
    public class SonosRuleActionRunner : BaseRuleActionRunner<SonosRuleAction>
    {
        protected override async Task ExecuteActionAsync(SonosRuleAction action)
        {
            if (action.CommandType == SonosActionType.Favorite)
            {
                await new SonosHttpService().PlayFavorite(action.Name, action.Parameter);
            }
            else if (action.CommandType == SonosActionType.Pause) {
                await new SonosHttpService().Pause(action.Name);
            }
            else
            {
                Logger.ErrorFormat("Unknown sonos command type {0}", action.CommandType);
            }
        }
    }
}
