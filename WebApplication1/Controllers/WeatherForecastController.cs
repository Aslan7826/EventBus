using EventBusCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StartEventBusTest.EventBusSet;
using StartEventBusTest.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        IEventBus _IEventBus;
        public WeatherForecastController(IEventBus eventBus) 
        {
            _IEventBus = eventBus;
        } 

        [HttpGet]
        public void Get()
        {
            var eventObj = new ShowEvent() { Id = 1 };

            //var data = new DataHandler().Start(eventObj); 
             

            _IEventBus.Publish(eventObj, EventKey.Start);
            _IEventBus.Publish(eventObj, EventKey.Stop);
        }
    }
}
