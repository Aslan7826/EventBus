using EventBusCore.Events;
using EventBusCore.Handler;
using EventBusCore.HandlerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusCore
{
    public class EventBus :IEventBus
    {
        private IEventHandlerManager _IEventHandlerManager;
        private IServiceProvider _IServiceProvider;
        public EventBus(IEventHandlerManager eventHandler, IServiceProvider serviceProvider)
        {
            _IEventHandlerManager = eventHandler;
            _IServiceProvider = serviceProvider;
        }
        /// <summary>
        /// 發行動作
        /// 有執行成功回傳true 
        /// 找不到對象要觸發的動作回傳false
        /// </summary>
        /// <typeparam name="TEvent">事件的對象</typeparam>
        /// <param name="event">事件的對象實作</param>
        /// <param name="eventStepType">事件的前中後</param>
        /// <returns></returns>
        public bool Publish<TEvent>(TEvent @event, Enum enumKey) where TEvent : IEventBase
        {
            _IEventHandlerManager.EventEnumKey = GetEnumKey(enumKey);
            if (_IEventHandlerManager.HasSubscriptionsForEvent<TEvent>())
            {
                var handlers = _IEventHandlerManager.GetEventHandlerTypes<TEvent>()
                               .OrderBy(o => o.Order)
                               .Select(types =>
                               {
                                   if (_IServiceProvider.GetService(types.EventHandlerTypes) is IEventHandler<TEvent> handler)
                                   {
                                       return handler;
                                   }
                                   return null;
                               })
                               .Where(o => o != null)
                               .ToList();
                if (handlers.Count > 0)
                {
                    var handlerTasks = handlers
                                       .Select(handler => handler.Handle(@event))
                                       .ToList();
                    Task.WhenAll(handlerTasks).ConfigureAwait(false);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 增加一個事件的對象與其要處發的事件
        /// </summary>
        /// <typeparam name="TEvent">事件的對象</typeparam>
        /// <typeparam name="TEventHandler">要處發的事件</typeparam>
        /// <param name="enumKey">事件的前中後</param>
        /// <returns></returns>
        public bool Subscribe<TEvent, TEventHandler>(Enum enumKey,  int order)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            _IEventHandlerManager.EventEnumKey = GetEnumKey(enumKey);
            return _IEventHandlerManager.AddSubscription<TEvent, TEventHandler>(order);
        }
        /// <summary>
        /// 增加一個事件的對象與其要處發的事件
        /// </summary>
        /// <param name="eventType">事件的對象</typeparam>
        /// <param name="eventHandlerType">要處發的事件</typeparam>
        /// <param name="enumKey">事件的前中後</param>
        /// <returns></returns>
        public bool Subscribe(Type eventType,Type eventHandlerType,Enum enumKey,int order) 
        {
            if( CheckEventType(eventType) && CheckEventHandlerType(eventHandlerType)) 
            {
                _IEventHandlerManager.EventEnumKey = GetEnumKey(enumKey);
                 return _IEventHandlerManager.AddSubscription(eventType, eventHandlerType,order);
            }
            return false;
        }
        /// <summary>
        /// 移除一個事件的對象要處發的事件
        /// </summary>
        /// <typeparam name="TEvent">事件的對象</typeparam>
        /// <typeparam name="TEventHandler">要移除的事件</typeparam>
        ///  <param name="eventStepType">事件的前中後</param>
        /// <returns></returns>
        public bool Unsubscribe<TEvent, TEventHandler>(Enum enumKey,  int? order = null)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            _IEventHandlerManager.EventEnumKey = GetEnumKey(enumKey);
            return _IEventHandlerManager.RemoveSubscription<TEvent, TEventHandler>(order);
        }
        /// <summary>
        /// 移除一個事件的對象要處發的事件
        /// </summary>
        /// <typeparam name="TEvent">事件的對象</typeparam>
        /// <typeparam name="TEventHandler">要移除的事件</typeparam>
        ///  <param name="eventStepType">事件的前中後</param>
        /// <returns></returns>
        public bool Unsubscribe(Type eventType,Type eventHandlerType,Enum enumKey, int? order = null)
        {
            if(CheckEventType(eventType) && CheckEventHandlerType(eventHandlerType)) 
            {
                _IEventHandlerManager.EventEnumKey = GetEnumKey(enumKey);
                return _IEventHandlerManager.RemoveSubscription(eventType,eventHandlerType,order);
            }
            return false;
        }
        /// <summary>
        /// 移除該Event 下所有的Handler
        /// </summary>
        /// <returns></returns>
        public bool Clear<TEvent>(Enum enumKey) where TEvent : IEventBase
        {
            _IEventHandlerManager.EventEnumKey = enumKey.ToString();
            return _IEventHandlerManager.Clear<TEvent>();
        }
        /// <summary>
        /// 移除所有Handler
        /// </summary>
        /// <returns></returns>
        public bool ClearAll()
        {
            return _IEventHandlerManager.ClearAll();
        }

        /// <summary>
        /// 建立Key
        /// </summary>
        /// <param name="enumKey"></param>
        /// <param name="eventStep"></param>
        /// <returns></returns>
        private string GetEnumKey(Enum enumKey)
        {
            return $"{enumKey}";
        }


        private bool CheckEventType(Type eventType) 
        {
            return typeof(IEventBase).IsAssignableFrom(eventType);
        }
        private bool CheckEventHandlerType(Type handlerType) 
        {
            return handlerType.GetInterfaces()
                   .Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IEventHandler<>));
        }
    }
}
