using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using EventBusCore;
using EventBusCore.DependencyManagement;
using EventBusCore.Events;
using EventBusCore.Handler;
using StartEventBusTest.EventBusSet;
using StartEventBusTest.Handler;

namespace StartEventBusTest
{
    public class EventBusRegistrar : IEventBusRegistrar
    {
        IEventBus _IEventBus;
        public EventBusRegistrar(IEventBus eventBus) 
        {
            _IEventBus = eventBus;
        }
        public void Do()
        {
            
            var allHanlder = AppDomain.CurrentDomain.GetAssemblies()
                             .Where(o=>!o.IsDynamic)
                             .SelectMany(o => o.ExportedTypes)
                             .Where(o =>o.BaseType!=null 
                                     && o.BaseType.IsGenericType 
                                     && o.BaseType.GetGenericTypeDefinition() == typeof(AllMethodsHandler<>) )
                             .ToList();
            var hanlderStep = new List<(string hanlderName, EventKey eventKey)>()
            {
                (hanlderName:"EventKeyStartHanlder" ,eventKey: EventKey.Start),
                (hanlderName:"EventKeyStopHandler" ,eventKey: EventKey.Stop),
            };
            foreach (var handler in allHanlder) 
            {
                var eventBase = handler.BaseType.GenericTypeArguments.FirstOrDefault(o=>typeof(IEventBase).IsAssignableFrom(o));
                if(eventBase!=null ) 
                {
                    
                    var handlers = handler.BaseType.GetNestedTypes()
                                   .Where(o => o.IsGenericType 
                                            && o.GetInterfaces().Any(x=>x.IsGenericType 
                                                                  && x.GetGenericTypeDefinition() == typeof(IEventHandler<>)) 
                                          );
                    
                    hanlderStep.ForEach(step =>
                   {
                       var hanlderType = handlers.First(o => o.Name == step.hanlderName);
                       _IEventBus.Subscribe(eventBase, hanlderType.MakeGenericType(eventBase), step.eventKey, 1);
                       
                   });
                }
            }                

            //_IEventBus.Subscribe<ShowEvent, ShowHandler>(EventKey.Start,1);
            //_IEventBus.Subscribe<ShowEvent, DataHandler.EventKeyStartHanlder>(EventKey.Start,1);
            //_IEventBus.Subscribe<ShowEvent, DataHandler.EventKeyStopHandler>(EventKey.Stop,1);

        }
    }
}
