using Automato.Model;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Processors
{
    /// <summary>
    /// Checks if a user-related rule is active
    /// </summary>
    public class UserRuleProcessor : BaseRuleProcessor<UserRule>
    {
        public override bool IsRuleActive(UserRule rule, Model.HomeStates.HomeState state)
        {
            // "Anyone is home"
            if (rule.UserState.UserId == Constants.UserIds.Anyone)
            {
                return state.Users.Any(u => u.IsHome == rule.UserState.IsHome);
            }
            // "No one is home"
            else if (rule.UserState.UserId == Constants.UserIds.NoOne)
            {
                return !state.Users.Any(u => u.IsHome == rule.UserState.IsHome);
            }
            // "Jeff is home"
            else
            {
                var user = state.Users.FirstOrDefault(u => u.UserId == rule.UserState.UserId);

                return user != null && user.IsHome == rule.UserState.IsHome;
            }
        }
    }
}
