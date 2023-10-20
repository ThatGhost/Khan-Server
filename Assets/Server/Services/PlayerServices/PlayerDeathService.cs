using System;
using Zenject;

namespace Server.Services
{
    public class PlayerDeathService : IPlayerDeathService, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;

        public void Dispose()
        {
            _signalBus.Subscribe<OnDeathSignal>(x => onDeath(x.connectionId));
        }

        public void Initialize()
        {
            _signalBus.Unsubscribe<OnDeathSignal>(x => onDeath(x.connectionId));
        }

        private void onDeath(int connection)
        {

        }
    }
}
