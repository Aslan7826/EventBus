using EventBusCore.Events;
using EventBusCore.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusCore
{
    /// <summary>
    /// 參考 
    /// https://www.cnblogs.com/weihanli/p/implement-a-simple-event-bus.html
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 增加一個對象與其Handler 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="order">排序</param>
        /// <returns></returns>
        bool Subscribe<TEvent, TEventHandler>(Enum enumKey,  int order)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// 移除一個對象與其Handler  ,order 指定排序 ,若沒有則刪除全部指定的Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="order">排序</param>
        /// <returns></returns>
        bool Unsubscribe<TEvent, TEventHandler>(Enum enumKey, int? order = null)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// 發行動作
        /// 有執行成功回傳true 
        /// 找不到對象要觸發的動作回傳false
        /// </summary>
        /// <typeparam name="TEvent">事件的對象</typeparam>
        /// <param name="event">事件的對象實作</param>
        /// <returns></returns>
        bool Publish<TEvent>(TEvent @event, Enum enumKey) where TEvent : IEventBase;


        /// <summary>
        /// 移除該Event 下所有的Handler
        /// </summary>
        /// <returns></returns>
        public bool Clear<TEvent>(Enum enumKey) where TEvent : IEventBase;

        /// <summary>
        /// 移除所有Handler
        /// </summary>
        /// <returns></returns>
        public bool ClearAll();
    }
}
