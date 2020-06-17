using System.Linq;
using System;
using System.Collections.Generic;
using CoVid.Models;
using CoVid.Processes;
using CoVid.Processes.PropertiesReader;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

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

        [HttpGet("datesfiltering/{data}")]
        public GeoZone GetWithDatesFiltering(string data)
        {
            string[] dataFiltering = data.Split(';'); 
            var toReturn = _oEuDataGetting.GetGeoZones().Find(x => x.geoID == dataFiltering[3]);
            if(toReturn is null)
            {
                _oEuDataGetting = InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter");
                toReturn = _oEuDataGetting.GetGeoZones().Find(x => x.geoID == dataFiltering[3]);
            }
            dataFiltering[1] = dataFiltering[1].Replace(dataFiltering[0], "/");
            dataFiltering[2] = dataFiltering[2].Replace(dataFiltering[0], "/");
            List<CoVid.Models.CoVidData> oCovidDateList = toReturn.dataList.ToList();
            var starDate = oCovidDateList.Find(oCoVidData => oCoVidData.date.date == dataFiltering[1]);
            var endDate = oCovidDateList.Find(oCoVidData => oCoVidData.date.date == dataFiltering[2]);
            oCovidDateList = oCovidDateList.FindAll(oCovidData => oCovidData.id >= starDate.id && oCovidData.id <= endDate.id);
            toReturn.dataList = new ConcurrentBag<CoVid.Models.CoVidData>(oCovidDateList);
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