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
        public override Task<bool> IsRuleActive(UserRule rule, Model.HomeStates.HomeState state)
        {
            bool isActive;

            // "Anyone is home"
            if (rule.UserState.UserId == Constants.UserIds.Anyone)
            {
                isActive = state.Users.Any(u => u.IsHome == rule.UserState.IsHome);
            }
            // "No one is home"
            else if (rule.UserState.UserId == Constants.UserIds.NoOne)
            {
                isActive = !state.Users.Any(u => u.IsHome == rule.UserState.IsHome);
            }
            // "Jeff is home"
            else
            {
                var user = state.Users.FirstOrDefault(u => u.UserId == rule.UserState.UserId);

                isActive = user != null && user.IsHome == rule.UserState.IsHome;
            }

            return Task.FromResult(isActive);
        }
    }
}
