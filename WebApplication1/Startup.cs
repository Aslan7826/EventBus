using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBusCore;
using EventBusCore.DependencyManagement;
using EventBusCore.Events;
using EventBusCore.Handler;
using EventBusCore.HandlerManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StartEventBusTest;
using StartEventBusTest.EventBusSet;
using StartEventBusTest.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.Build();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.ApplicationServices.GetAutofacRoot().Resolve<IEventBusRegistrar>().Do();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<EventBusRegistrar>().As<IEventBusRegistrar>();
            builder.RegisterType<EventBus>().As<IEventBus>();
            builder.RegisterType<EventHandlerManager>().As<IEventHandlerManager>();
            //builder.RegisterType<ShowHandler>();
            
            

            var allHanlder = AppDomain.CurrentDomain.GetAssemblies()
                 .Where(o => !o.IsDynamic)
                 .SelectMany(o => o.ExportedTypes)
                 .Where(o => o.BaseType != null
                         && o.BaseType.IsGenericType
                         && o.BaseType.GetGenericTypeDefinition() == typeof(AllMethodsHandler<>))
                 .ToList();
            var hanlderStep = new List<string>()
            {
                "EventKeyStartHanlder",
                "EventKeyStopHandler" 
            };

            foreach (var handler in allHanlder)
            {
                var eventBase = handler.BaseType.GenericTypeArguments.FirstOrDefault(o => typeof(IEventBase).IsAssignableFrom(o));
                if (eventBase != null)
                {
                    builder.RegisterType(handler).As(handler.BaseType);
                    var handlers = handler.BaseType.GetNestedTypes()
                                   .Where(o => o.IsGenericType
                                            && o.GetInterfaces().Any(x => x.IsGenericType
                                                                       && x.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                                    );

                    hanlderStep.ForEach(step =>
                   {
                       var hanlderType = handlers.First(o => o.Name == step);
                       builder.RegisterType(hanlderType.MakeGenericType(eventBase));
                   });
                }
            }


            // builder.RegisterGeneric(typeof(AllMethodsHandler<>));
            /*builder.RegisterType(typeof(DataHandler)).As(types.BaseType);
            builder.RegisterType<DataHandler.EventKeyStartHanlder>();
            builder.RegisterType<DataHandler.EventKeyStopHandler>();*/
            
        }


    }
}
