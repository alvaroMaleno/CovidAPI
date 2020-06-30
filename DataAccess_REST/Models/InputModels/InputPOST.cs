using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class InputPOST
    {
        [JsonPropertyName("method")]
        public string method{get;set;}

        [JsonPropertyName("covid_data")]
        public CovidData _oCovidData{get;set;}
        
        public InputPOST(){}
        
        public InputPOST(string pMethod, CovidData pCovidData)
        {
            this.method = pMethod;
            this._oCovidData = pCovidData;
        }
    }
}