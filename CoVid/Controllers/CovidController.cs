using System.Collections.Generic;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Controllers.DAOs.CreateTableOperations;
using CoVid.DAOs.Abstracts;
using CoVid.Models;
using CoVid.Models.InputModels;
using CoVid.Processes;
using CoVid.Processes.PropertiesReader;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class CovidController : Controller
    {
        private CovidDAO _oCovidDao = CovidDAOPostgreImpl.GetInstance();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }    
        
        // [HttpGet("{id}")]
        // public GeoZone Get(string id)
        // {
        //     return InitDataGetting.GetInstance(_URL, "EUDataCenterJSONDataGetter").GetGeoZones().Find(x => x.geoID == id);
        // }    
        
        [HttpPost]
        public List<GeoZone> Post([FromBody]InputPOST pPOST)
        {
            List<GeoZone> oListToReturn = new List<GeoZone>();
            var pass = pPOST?.oUser?.pass?.ToLower() ?? string.Empty;
            if(pass.Contains("secret"))
            {
                _oCovidDao.GetGeoZoneData(pPOST.oCovidData, oListToReturn);
            }
            else
            {
                GeoZone oGeozone = new GeoZone();
                oGeozone.name = "Pass Error";
                oListToReturn.Add(oGeozone);
            }
            
            return oListToReturn;
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