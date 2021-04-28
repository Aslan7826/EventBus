using System;
using System.Collections.Generic;
using System.Text;
using EventBusCore;
using EventBusCore.DependencyManagement;
using StartEventBusTest.EventBusSet;
using StartEventBusTest.Handler;

namespace StartEventBusTest
{
    public class EventBusRegistrar : IEventBusRegistrar
    {
        IEventBus _IEventBus;
        public EventBusRegistrar(IEventBus eventBus) 
        {
            _IEventBus = eventBus;
        }
        public void Do()
        {
            //_IEventBus.Subscribe<ShowEvent, ShowHandler>(EventKey.Start,1);
            _IEventBus.Subscribe<ShowEvent, DataHandler.EventKeyStartHanlder>(EventKey.Start,1);
            _IEventBus.Subscribe<ShowEvent, DataHandler.EventKeyStopHandler>(EventKey.Stop,1);

        }
    }
}
