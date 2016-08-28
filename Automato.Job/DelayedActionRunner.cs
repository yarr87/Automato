using Automato.Logic;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
using Automato.Model.Rules;
using log4net;
using Raven.Abstractions.Data;
using Raven.Client.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automato.Job
{
    /// <summary>
    /// Listens for new DelayedAction documents, and schedules them to run when they are received
    /// </summary>
    public class DelayedActionRunner
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // TODO: build a UI to create delayed actions, deploy and test
        
        public void Start()
        {
            Logger.Debug("Starting up delayed action runner");

            Context.DocumentStore.Value
                .Changes()
                .ForDocumentsOfType<DelayedAction>()
                .Subscribe(new DelayedActionObserver());
        }

        class DelayedActionObserver : IObserver<DocumentChangeNotification>
        {
            public void OnCompleted()
            {
                Logger.Debug("OnCompleted");
            }

            public void OnError(Exception error)
            {
                Logger.Error("DelayedActionObserver", error);
            }

            public void OnNext(DocumentChangeNotification value)
            {
                if (value.Type == DocumentChangeTypes.Put)
                {
                    var store = new DelayedActionStore();

                    Logger.DebugFormat("Got delayed action with id {0}", value.Id);

                    var delayedAction = store.GetById(value.Id);

                    if (delayedAction.StartTime >= DateTime.Now)
                    {
                        Logger.DebugFormat("Executing action {0} immediately", value.Id);
                        ProcessDelayedAction(delayedAction);
                    }
                    else
                    {
                        var timeSpan = DateTime.Now - delayedAction.StartTime;

                        Logger.DebugFormat("Queuing action {0} to execute in {1}", value.Id, timeSpan);

                        // TODO: this is a single-use timer.  Will this cause a memory leak?
                        var timer = new Timer(new TimerCallback(ProcessDelayedAction), delayedAction, timeSpan, Timeout.InfiniteTimeSpan);
                    }

                    // Don't need this anymore once it's run
                    store.DeleteById(value.Id);
                }
            }

            private void ProcessDelayedAction(object state)
            {
                ProcessDelayedActionAsync(state as DelayedAction).Wait();
            }

            private async Task ProcessDelayedActionAsync(DelayedAction delayedAction)
            {
                try
                {
                    await new RuleRunner().RunRule(delayedAction);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error processing delayed action", ex);
                }
            }
        }
    }
}
