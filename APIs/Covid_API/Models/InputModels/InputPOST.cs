using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class InputPOST
    {
        [JsonPropertyName("token")]
        public string token{get;set;}
        
        [JsonPropertyName("covid_data")]
        public CovidData oCovidData{get;set;}

        public InputPOST(){}
        public InputPOST(string pToken, CovidData pCovidData)
        {
            this.token = pToken;
            this.oCovidData = pCovidData;
        }
    }
}