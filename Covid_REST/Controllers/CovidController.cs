using System;
using System.Collections.Generic;
using CoVid.DAOs.Abstracts;
using CoVid.Models;
using CoVid.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class CovidController : Controller
    {
        private readonly string _HTTP_ERROR = "Http Error: ";
        private CovidDAO _oCovidDao = CovidDAOPostgreImpl.GetInstance();

        [HttpPost]
        public object Post([FromBody]InputPOST pPOST)
        {
            object oToReturn;
            
            bool isAuthenticated = this.AuthenticateUser(pPOST.oUser);
            if(!isAuthenticated)
            {
                oToReturn = "User/Pass Error";
                return oToReturn;
            }

            var dataType = pPOST?.oCovidData?.dataType.ToLower() ?? string.Empty;
            switch (dataType)
            {
                case "getcountries":
                    this.GetCountries(out oToReturn);
                    break;
                case "getdates":
                    this.GetDates(out oToReturn);
                    break;
                default:
                    this.GetGeoZoneData(pPOST.oCovidData, out oToReturn);
                    break;
            }

            return oToReturn;
        }

        private void GetGeoZoneData(CovidData oCovidData, out object oToReturn)
        {
            if(oCovidData is null)
            {
                oToReturn = String.Concat(
                    _HTTP_ERROR ,
                     System.Net.HttpStatusCode.BadRequest);
                return;
            }
            
            this.SetDateFormat(oCovidData);

            bool isDemandingAllGeoZoneData = false;
            foreach (var country in oCovidData.oCountryList)
            {
                if(country == "*")
                {
                    isDemandingAllGeoZoneData = true;
                    break;
                }
            }

            List<GeoZone> oListToReturn = new List<GeoZone>();
            if(isDemandingAllGeoZoneData)
            {
                _oCovidDao.GetAllGeoZoneData(oCovidData, oListToReturn);
            }
            else
            {
                _oCovidDao.GetGeoZoneData(oCovidData, oListToReturn);
            }

            if(oListToReturn.Count == 0)
            {
                oToReturn = String.Concat(
                    _HTTP_ERROR , 
                    System.Net.HttpStatusCode.BadRequest);
                return;
            }

            oToReturn = oListToReturn;
        }

        private void SetDateFormat(CovidData oCovidData)
        {
            oCovidData.oDates.startDate = oCovidData.oDates.startDate.Replace(oCovidData.oDates.separator, "/");
            oCovidData.oDates.endDate = oCovidData.oDates.endDate.Replace(oCovidData.oDates.separator, "/");
        }

        private void GetDates(out object oToReturn)
        {
            List<CovidDate> oListToReturn = new List<CovidDate>();
            _oCovidDao.GetAllDates(oListToReturn);
            oToReturn = oListToReturn;
        }

        private void GetCountries(out object oToReturn)
        {
            List<GeoZone> oListToReturn = new List<GeoZone>();
            _oCovidDao.GetAllCountries(oListToReturn);
            oToReturn = oListToReturn;
        }

        private bool AuthenticateUser(Models.InputModels.User oUser)
        {
            var pass = oUser?.pass?.ToLower() ?? string.Empty;
            if(pass.Contains("secret"))
            {
                return true;
            }
            return false;
        }
    }
}