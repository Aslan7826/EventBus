
using EventBusCore.Events;
using EventBusCore.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusCore.HandlerManager
{
    public interface IEventHandlerManager
    {
        /// <summary>
        /// 增加Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <returns></returns>
        bool AddSubscription<TEvent, TEventHandler>(int order)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;
        /// <summary>
        /// 移除Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <returns></returns>
        bool RemoveSubscription<TEvent, TEventHandler>(int? order = null)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;
        /// <summary>
        /// 確認是否有能用的Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        bool HasSubscriptionsForEvent<TEvent>() where TEvent : IEventBase;
        /// <summary>
        /// 取出所有能用的Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        ICollection<EventHandlerType> GetEventHandlerTypes<TEvent>() where TEvent : IEventBase;
        /// <summary>
        /// 移除該Event 下所有的Handler
        /// </summary>
        /// <returns></returns>
        bool Clear<TEvent>() where TEvent : IEventBase;

        /// <summary>
        /// 移除所有Handler
        /// </summary>
        /// <returns></returns>
        bool ClearAll();
        /// <summary>
        /// 取得EventKey
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        string GetEventKey<TEvent>() where TEvent : IEventBase;

        /// <summary>
        /// 指定的字典開頭
        /// </summary>
        string EventEnumKey { get; set; }

    }
}
