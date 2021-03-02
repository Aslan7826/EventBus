using EventBusCore.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBusCore.Handler
{
    public interface IEventHandler<in TEvent> where TEvent : IEventBase
    {
        /// <summary>
        /// 處理動作
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Handle(TEvent @event);
    }
}
