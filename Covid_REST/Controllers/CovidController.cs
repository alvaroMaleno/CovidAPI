using System;
using System.Threading.Tasks;
using CoVid.Models.InputModels;
using CoVid.Models.OutputModels;
using CoVid.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class CovidController : Controller
    {
        private readonly string _HTTP_ERROR = "Http Error: ";
        private readonly string _URL_DATA_REST = "https://localhost:5005/CovidDataBase";

        [HttpPost]
        public object Post([FromBody]InputPOST pPOST)
        {
            object oToReturn = null;
            
            bool isAuthenticated = this.AuthenticateUser(pPOST.oUser);
            if(!isAuthenticated)
            {
                oToReturn = "User/Pass Error";
                return oToReturn;
            }
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
                oToReturn = this.GetFromDAO(oCovidData, "GetAllGeoZoneData").Result;
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
            object oListToReturn = null;
            DAOModelPOST oDAOModelPOST = new DAOModelPOST(pMethod, pCovidData);
            oListToReturn = await UtilsJSON.GetInstance().DeserializeFromPOSTUrl(oListToReturn, this._URL_DATA_REST, oDAOModelPOST);
            return oListToReturn;
        }

        private bool AuthenticateUser(Models.InputModels.User oUser)
        {
            var pass = oUser?.pass?.ToLower() ?? string.Empty;
            if(pass.Contains("secret"))
                return true;
            
            return false;
        }
    }
}