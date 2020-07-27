using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoVid.Cache;
using CoVid.Models.InputModels;
using CoVid.Models.OutputModels;
using CoVid.Utils;
using Covid_REST.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class CovidController : Controller
    {
        private readonly string _HTTP_ERROR = "Http Error: ";
        private readonly string _URL_DATA_REST = "https://localhost:5005/CovidDataBase";
        private readonly string _URL_SECURITY_REST = "https://localhost:5003/Security";
        private CovidCache _oCovidCache = CovidCache.GetInstance();

        [HttpPost]
        [Authorize]
        public object Post([FromBody]InputPOST pPOST)
        {
            object oToReturn = null;

            string token = UtilsJSON.GetInstance().GetFromUrl(_URL_SECURITY_REST);
            token = Convert.ToBase64String(Encoding.ASCII.GetBytes(token));

            // if(pPOST.token != token)
            // {
            //     return "Please, authorize.";
            // }

            if(pPOST.oCovidData is null)
            {
                oToReturn = String.Concat(
                    _HTTP_ERROR ,
                     System.Net.HttpStatusCode.BadRequest);
                return oToReturn;
            }

            var dataType = pPOST?.oCovidData?.dataType.ToLower() ?? string.Empty;
            switch (dataType)
            {
                case "getcountries":
                    oToReturn = this.GetFromDAO(pPOST.oCovidData, "GetAllCountries").Result;
                    break;
                case "getdates":
                    oToReturn = this.GetFromDAO(pPOST.oCovidData, "GetAllDates").Result;
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
                oToReturn = this.GetFromDAO(oCovidData, "GetGeoZoneData").Result;
        }

        private void SetDateFormat(CovidData oCovidData)
        {
            string RIGHT_BAR = "/";
            if(oCovidData.oDates.separator == RIGHT_BAR)
                return;

            oCovidData.oDates.startDate = oCovidData.oDates.startDate.Replace(oCovidData.oDates.separator, RIGHT_BAR);
            oCovidData.oDates.endDate = oCovidData.oDates.endDate.Replace(oCovidData.oDates.separator, RIGHT_BAR);
        }

        private async Task<object> GetFromDAO(CovidData pCovidData, string pMethod)
        {
            DAOModelPOST oDAOModelPOST = new DAOModelPOST(pMethod, pCovidData);
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(this._URL_DATA_REST, oDAOModelPOST);
        }

        private async Task<string> AuthenticateUser(Models.InputModels.User pUser)
        {
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(this._URL_SECURITY_REST, pUser);
        }

    }
}