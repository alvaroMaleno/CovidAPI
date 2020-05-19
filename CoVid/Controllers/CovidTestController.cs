using System.Collections.Generic;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Controllers.DAOs.CreateTableOperations;
using CoVid.Models;
using CoVid.Processes;
using CoVid.Processes.PropertiesReader;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class CovidTestController : Controller
    {
        private static readonly string _URL = "https://opendata.ecdc.europa.eu/covid19/casedistribution/json/";

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }    
        
        [HttpGet("{id}")]
        public GeoZone Get(string id)
        {
            return InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter").GetGeoZones().Find(x => x.geoID == id);
        }    
        
        [HttpPost]
        public List<GeoZone> Post([FromBody]User user)
        {
            if(user.pass.Contains("Secret"))
            {
                return InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter").GetGeoZones();
            }
            GeoZone oGeozone = new GeoZone();
            oGeozone.name = "Pass Error";
            return new List<GeoZone>(){oGeozone};
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