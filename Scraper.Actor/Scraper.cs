using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using PageProcessor.PageProcessor;
using PageProcessor.ServiceFactory;
using Scraper.Interfaces;

namespace Scraper.Actor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Scraper : Microsoft.ServiceFabric.Actors.Runtime.Actor, IRemindable, IScraper
    {
        /// <summary>
        /// Initializes a new instance of Scraper
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Scraper(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId){ }
        
        private IServiceFactory<IPageProcessor> _pageProcessorFactory;
        public IServiceFactory<IPageProcessor> PageProcessorFactory {
            get => _pageProcessorFactory ?? (_pageProcessorFactory = new DefaultServiceFactory());
            set => _pageProcessorFactory = value;
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, Constants.ActorActivatedMessage);
            return Task.FromResult(true);
        }
        public async Task StartWork()
        {
            var page = Id.GetLongId();
            var reminderName = Constants.ReminderName.Replace(Constants.IdToken, page.ToString());

            var reminderRegistration = await this.RegisterReminderAsync(
                reminderName,
                BitConverter.GetBytes(default(int)),
                TimeSpan.FromSeconds(default(int)),
                TimeSpan.FromDays(Convert.ToInt64(GetConfigParameter(Constants.RunPerDaysKey))));
                
            
            await StateManager.TryAddStateAsync(Constants.PageKey, page);
            await StateManager.TryAddStateAsync(Constants.PageShiftKey, Convert.ToInt64(GetConfigParameter(Constants.PageShiftKey)));
        }
        
        private string GetConfigParameter(string key)
        {
            var config = ActorService.Context.CodePackageActivationContext.GetConfigurationPackageObject(Constants.ConfigKey);
            return config.Settings.Sections[Constants.ScraperActorServiceConfigKey].Parameters[key].Value;
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.StartsWith(ReminderNameHeader))
            {
                var page =  await StateManager.GetStateAsync<long>(Constants.PageKey);
                var pageShift = await StateManager.GetStateAsync<long>(Constants.PageShiftKey);

                while (await PageProcessorFactory.Service.ProcessPage(page))
                {
                    await StateManager.TryAddStateAsync(Constants.PageKey, page);
                    page += pageShift;
                }
            }
        }

        public string ReminderNameHeader => Constants.ReminderName.Replace(Constants.IdToken, string.Empty);
    }
}
