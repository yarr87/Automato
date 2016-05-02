using Automato.Integration.Model;
using Automato.Integration.Router;
using Automato.Logic;
using Automato.Logic.HomeStates;
using Automato.Logic.Stores;
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
    /// Periodically logs the current home state
    /// </summary>
    public class HomeStateLogger
    {
        private TimeSpan timeSpan = TimeSpan.FromSeconds(Int32.Parse(ConfigurationManager.AppSettings["HomeStateLogFrequencyInSeconds"]));
        private Timer _timer;
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Start()
        {
            Logger.Debug("Starting up home state logger");

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
                var homeState = await new HomeStateManager().GetCurrentHomeState();
                new HomeStateStore().Save(homeState);
            }
            catch (Exception ex)
            {
                Logger.Error("Error logging home state", ex);
            }
        }
    }
}
