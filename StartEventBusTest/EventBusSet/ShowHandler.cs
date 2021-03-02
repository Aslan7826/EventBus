using EventBusCore.Handler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StartEventBusTest.EventBusSet
{
    public class ShowHandler : IEventHandler<ShowEvent>
    {
        public Task Handle(ShowEvent @event)
        {
            Console.WriteLine($"{@event.Id}-{@event.DateTime}");
            return Task.FromResult(true); 
        }
    }
}
