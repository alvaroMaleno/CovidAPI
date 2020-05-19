using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{
   [Route("api/[controller]")]
    public class HelloController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "HelloWorld";
        }    
        
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }    
        
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }    
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }    
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}