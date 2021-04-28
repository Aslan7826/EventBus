using StartEventBusTest.EventBusSet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StartEventBusTest.Handler
{
    public class DataHandler : AllMethodsHandler<ShowEvent>  
    {
        public override Task EventStart(ShowEvent @event)
        {
            Console.WriteLine($"{@event.Id}-{@event.DateTime}-DataHandler");
            return Task.FromResult(true);
        }

        public override Task EventStop(ShowEvent @event)
        {
            Console.WriteLine($"{@event.Id}-{@event.DateTime}-DataHandler");
            return Task.FromResult(true);
        }
    }
}
