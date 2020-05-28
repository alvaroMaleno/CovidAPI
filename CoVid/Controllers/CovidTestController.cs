using System.Collections.Generic;
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
        private InitDataGetting _oEuDataGetting = InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter");

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }    
        
        [HttpGet("{id}")]
        public GeoZone Get(string id)
        {
            var toReturn = _oEuDataGetting.GetGeoZones().Find(x => x.geoID == id);
            if(toReturn is null)
            {
                _oEuDataGetting = InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter");
                toReturn = _oEuDataGetting.GetGeoZones().Find(x => x.geoID == id);
            }
            return toReturn;
        }    
        
        [HttpPost]
        public List<GeoZone> Post([FromBody]User user)
        {
            if(user.pass.Contains("Secret"))
            {
                var toReturn = _oEuDataGetting.GetGeoZones();
                if(toReturn is null)
                {
                    _oEuDataGetting = InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter");
                    toReturn = _oEuDataGetting.GetGeoZones();
                }
                return toReturn;
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