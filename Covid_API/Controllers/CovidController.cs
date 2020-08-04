using System;
using System.Threading.Tasks;
using CoVid.Cache;
using CoVid.Models.InputModels;
using CoVid.Models.OutputModels;
using Covid_REST.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class CovidController : Controller
    {
        private CovidCache _oCovidCache = CovidCache.GetInstance();

        [HttpPost]
        [Authorize]
        public object Post([FromBody]InputPOST pPOST)
        {
            object oToReturn = null;

            if(pPOST.oCovidData is null)
            {
                oToReturn = String.Concat(
                    "Http Error: " ,
                     System.Net.HttpStatusCode.BadRequest);
                return oToReturn;
            }

            var dataType = pPOST?.oCovidData?.dataType.ToLower() ?? string.Empty;
            switch (dataType)
            {
                case "getcountries":
                    oToReturn = this.GetFromDAO(pPOST.oCovidData, UtilsConstants.POSTMethodsConstants.GET_ALL_COUNTRIES).Result;
                    break;
                case "getdates":
                    oToReturn = this.GetFromDAO(pPOST.oCovidData, UtilsConstants.POSTMethodsConstants.GET_ALL_DATES).Result;
                    break;
                default:
                    this.GetGeoZoneData(pPOST.oCovidData, out oToReturn);
                    break;
            }

            return oToReturn;
        }

        private void GetGeoZoneData(CovidData oCovidData, out object oToReturn)
        {
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

            if(isDemandingAllGeoZoneData)
                _oCovidCache.GetListFilteredByDate(
                    oCovidData.oDates.startDate, oCovidData.oDates.endDate, out oToReturn);
            else
                oToReturn = this.GetFromDAO(oCovidData, UtilsConstants.POSTMethodsConstants.GET_GEO_ZONE_DATA).Result;
        }

        private void SetDateFormat(CovidData oCovidData)
        {
            if(oCovidData.oDates.separator == UtilsConstants.StringConstants.RIGHT_BAR)
                return;

            oCovidData.oDates.startDate = oCovidData.oDates.startDate.Replace(
                oCovidData.oDates.separator, 
                UtilsConstants.StringConstants.RIGHT_BAR);

            oCovidData.oDates.endDate = oCovidData.oDates.endDate.Replace(
                oCovidData.oDates.separator, 
                UtilsConstants.StringConstants.RIGHT_BAR);
        }

        private async Task<object> GetFromDAO(CovidData pCovidData, string pMethod)
        {
            DAOModelPOST oDAOModelPOST = new DAOModelPOST(pMethod, pCovidData);
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(UtilsConstants.UrlConstants.URL_DATA_REST, oDAOModelPOST);
        }

    }
}