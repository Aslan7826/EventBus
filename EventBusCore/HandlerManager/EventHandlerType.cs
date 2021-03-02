using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusCore.HandlerManager
{
    public class EventHandlerType
    {
        public int Order { get; set; }
        public Type EventHandlerTypes { get; set; }
    }
}
