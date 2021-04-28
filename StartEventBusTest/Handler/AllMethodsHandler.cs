using EventBusCore.Events;
using EventBusCore.Handler;
using StartEventBusTest.EventBusSet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StartEventBusTest.Handler
{
    public abstract class AllMethodsHandler<T>  where T : IEventBase
    {

        public abstract Task EventStart(T @event);
        public abstract Task EventStop(T @event);

        public class EventKeyStopHandler : IEventHandler<T>
        {
            AllMethodsHandler<T> _AllHandler;
            public EventKeyStopHandler(AllMethodsHandler<T> allHandler)
            {
                this._AllHandler = allHandler;
            }
            public Task Handle(T @event) => _AllHandler.EventStop(@event);
        }
        public class EventKeyStartHanlder : IEventHandler<T>
        {
            AllMethodsHandler<T> _AllHandler;
            public EventKeyStartHanlder(AllMethodsHandler<T> allHandler)
            {
                this._AllHandler = allHandler;
            }
            public Task Handle(T @event) => _AllHandler.EventStart(@event);
        }


    }

}
