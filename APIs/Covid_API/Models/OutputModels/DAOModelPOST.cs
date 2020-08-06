using System.Text.Json.Serialization;
using CoVid.Models.InputModels;

namespace CoVid.Models.OutputModels
{
    public class DAOModelPOST
    {
         [JsonPropertyName("method")]
        public string method{get;set;}

        [JsonPropertyName("covid_data")]
        public CovidData _oCovidData{get;set;}

        public DAOModelPOST(){}
        public DAOModelPOST(string pMethod, CovidData pCovidData)
        {
            this.method = pMethod;
            this._oCovidData = pCovidData;
        }
    }
}