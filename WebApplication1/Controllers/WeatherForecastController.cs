using EventBusCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StartEventBusTest.EventBusSet;
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
             _IEventBus.Publish(new ShowEvent() { Id = 1 }, EventKey.Start);
        }
    }
}
