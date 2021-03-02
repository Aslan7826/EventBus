using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusCore.Events
{
    public class EventBase : IEventBase
    {
        public EventBase()
        {
            DateTime = DateTime.Now.ToLocalTime();

        }
        /// <summary>
        /// 編號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 事件時間
        /// </summary>
        public DateTime DateTime { get; }
        /// <summary>
        /// 事件名稱
        /// </summary>
        public string GetEventName { get; }
    }
}
