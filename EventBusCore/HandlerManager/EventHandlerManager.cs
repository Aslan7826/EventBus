using EventBusCore.Events;
using EventBusCore.Handler;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventBusCore.HandlerManager
{
    public class EventHandlerManager : IEventHandlerManager
    {
        /// <summary>
        /// 查詢用的字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, HashSet<EventHandlerType>> _eventHandlers = new ConcurrentDictionary<string, HashSet<EventHandlerType>>();


        /// <summary>
        /// 增加Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        ///<param name="order">傳入排序</param>
        /// <returns></returns>
        public bool AddSubscription<TEvent, TEventHandler>(int order)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventKey = GetEventKey<TEvent>();
            var addHandler = new EventHandlerType() { Order = order, EventHandlerTypes = typeof(TEventHandler) };
            if (_eventHandlers.ContainsKey(eventKey))
            {
                return _eventHandlers[eventKey].Add(addHandler);
            }
            else
            {
                return _eventHandlers.TryAdd(eventKey, new HashSet<EventHandlerType>() { addHandler });
            }
        }

        /// <summary>
        /// 取出所有能用的Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public ICollection<EventHandlerType> GetEventHandlerTypes<TEvent>() where TEvent : IEventBase
        {
            if (_eventHandlers.Count > 0)
            {
                var eventKey = GetEventKey<TEvent>();
                if (_eventHandlers.TryGetValue(eventKey, out var handlers))
                {
                    return handlers;
                }
            }
            return new EventHandlerType[0];
        }

        /// <summary>
        /// 確認是否有能用的Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public bool HasSubscriptionsForEvent<TEvent>() where TEvent : IEventBase
        {
            if (_eventHandlers.Count == 0)
                return false;

            var eventKey = GetEventKey<TEvent>();
            return _eventHandlers.ContainsKey(eventKey);
        }
        /// <summary>
        /// 移除該Event 底下Handler 有傳入order 則指定order,沒傳入則刪除全部同樣的Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        ///<param name="order">傳入排序</param>
        /// <returns></returns>
        public bool RemoveSubscription<TEvent, TEventHandler>(int? order = null)
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            if (_eventHandlers.Count == 0)
                return false;
            var eventKey = GetEventKey<TEvent>();
            if (_eventHandlers.ContainsKey(eventKey))
            {
                var thisType = typeof(TEventHandler);
                Predicate<EventHandlerType> bool1 = o => o.EventHandlerTypes == thisType;
                var select = bool1;
                if (order.HasValue)
                {
                    Predicate<EventHandlerType> bool2 = o => o.Order == order.Value;
                    select = o => bool1(o) && bool2(o);
                }
                return _eventHandlers[eventKey].RemoveWhere(select) > 0;
            }
            return false;
        }

        /// <summary>
        /// 移除該Event 下所有的Handler
        /// </summary>
        /// <returns></returns>
        public bool Clear<TEvent>() where TEvent : IEventBase
        {
            var result = true;
            var eventKeys = _eventHandlers
                            .Select(o => o.Key)
                            .Where(o => o.StartsWith(EventEnumKey));
            foreach (var eventKey in eventKeys)
            {
                var ans = _eventHandlers.TryRemove(eventKey, out var eventHandlers);
                result = result && ans;
            }
            return result;
        }

        /// <summary>
        /// 移除所有Handler
        /// </summary>
        /// <returns></returns>
        public bool ClearAll()
        {
            _eventHandlers.Clear();
            return true;
        }
        /// <summary>
        /// 取得EventKey
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public string GetEventKey<TEvent>() where TEvent : IEventBase
        {
            return $"{EventEnumKey}_{typeof(TEvent).FullName}";
        }
        /// <summary>
        /// 指定的字典開頭
        /// </summary>
        public string EventEnumKey { get; set; }
    }
}
