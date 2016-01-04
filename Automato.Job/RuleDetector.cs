using Automato.Logic.HomeStates;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
using Automato.Model.HomeStates;
using Automato.Model.Rules;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automato.Job
{
    /// <summary>
    /// Checks for rules every few minutes and runs the active ones
    /// </summary>
    public class RuleDetector
    {
        /// <summary>
        /// How often we check for new rules
        /// </summary>
        private TimeSpan timeSpan;

        private Timer _timer;
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HomeState _previousHomeState;
        private HomeStateManager _homeStateManager = new HomeStateManager();

        public void Start()
        {
            Logger.Debug("Starting up rule detector");

            timeSpan = TimeSpan.FromSeconds(Int32.Parse(ConfigurationManager.AppSettings["RuleCheckFrequencyInSeconds"]));

            _timer = new Timer(new TimerCallback(DoWork), null, TimeSpan.Zero, timeSpan);
        }

        private void DoWork(object state)
        {
            DoWorkAsync().Wait();
        }

        private async Task DoWorkAsync()
        {
            try
            {
                // TODO: run and test this
                var currentHomeState = await _homeStateManager.GetCurrentHomeState();

                if (_previousHomeState != null)
                {
                    var rules = new RuleStore().GetActive();

                    // Only include rules without triggers.  Those rules will be run when that triggered action happens.
                    // Ex: we don't want to run "When Jeff comes home after 6pm" here.  We do want to run "When Jeff is home after 6pm"
                    var nonTriggeredRules = rules.Where(r => !r.RuleDefinitions.Any(d => d.IsTriggered)).ToList();
                    var activeRules = new RulesProcessorEngine().GetNewlyActiveRules(nonTriggeredRules, _previousHomeState, currentHomeState);

                    await RunActiveRules(activeRules);
                }

                _previousHomeState = currentHomeState;
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing rules", ex);
            }
        }

        private async Task RunActiveRules(IEnumerable<Rule> rules)
        {
            var ruleRunner = new RuleRunner();

            foreach (var rule in rules)
            {
                Logger.DebugFormat("Running rule {0}", rule.Name);

                await ruleRunner.RunRule(rule);
            }
        }
    }
}
