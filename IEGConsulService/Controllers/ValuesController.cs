﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Consul;

namespace IEGConsulService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var ConsulClientConfig = new ConsulClientConfiguration
            {
                Address = new Uri("http://consul.westeurope.azurecontainer.io:8500/"),
                Datacenter = "dc1"              
    
            };
            var ConsulClient = new ConsulClient(ConsulClientConfig);

            var service = await ConsulClient.Catalog.Service("web");
            var services = service.Response;

            var rndService = services.FirstOrDefault();

            var serviceUrl = rndService.ServiceAddress;

            return new string[] { rndService.Address, rndService.ServiceName, rndService.ServiceAddress };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
