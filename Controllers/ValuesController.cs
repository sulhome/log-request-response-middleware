using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogReqResMiddleware.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"query string param is: {id}";
        }
        
        [HttpPost]
        public string Post([FromBody]JObject postedObject)
        {
            var postedObjectString = JsonConvert.SerializeObject(postedObject);            
            return $"value received: {postedObjectString}";
        }
    }
}