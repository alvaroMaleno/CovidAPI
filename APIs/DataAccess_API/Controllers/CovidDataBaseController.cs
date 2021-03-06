using System.Collections.Generic;
using CoVid.DAO;
using CoVid.DAOs.Abstracts;
using CoVid.Models;
using CoVid.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_DAO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidDataBaseController : ControllerBase
    {

        private readonly ILogger<CovidDataBaseController> _logger;
        private CovidDAO _oCovidDAO = CovidDAOPostgreImpl.GetInstance();

        public CovidDataBaseController(ILogger<CovidDataBaseController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public object Post(InputPOST pInputPOST)
        {
            if(pInputPOST is null){
                return null;
            }
            
            List<GeoZone> oListToReturn = new List<GeoZone>();
            switch (pInputPOST.method)
            {
                case "GetGeoZoneData":
                    _oCovidDAO.GetGeoZoneData(pInputPOST._oCovidData, oListToReturn);
                    break;
                case "GetAllGeoZoneData":
                    _oCovidDAO.GetAllGeoZoneData(pInputPOST._oCovidData, oListToReturn);
                    break;
                case "GetAllGeoZoneDataForAllDates":
                    _oCovidDAO.GetAllGeoZoneDataForAllDates(oListToReturn);
                    break;
                case "GetAllCountries":
                    _oCovidDAO.GetAllCountries(oListToReturn);
                    break;
                case "GetAllDates":
                    List<CovidDate> oListToReturnCovidDate = new List<CovidDate>();
                    _oCovidDAO.GetAllDates(oListToReturnCovidDate);
                    return oListToReturnCovidDate;
                    
                default:
                    return null;
            }
            
            return oListToReturn;
        }
    }
}
