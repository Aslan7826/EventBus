using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusCore.Events
{
    public interface IEventBase
    {
        /// <summary>
        /// 編號
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 事件時間
        /// </summary>
        DateTime DateTime { get; }
        /// <summary>
        /// 事件名稱
        /// </summary>
        string GetEventName { get; }
    }
}
