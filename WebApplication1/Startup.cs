using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBusCore;
using EventBusCore.DependencyManagement;
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
using System;
using System.Collections.Generic;
using System.Linq;
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
            builder.RegisterType<ShowHandler>();
        }


    }
}
