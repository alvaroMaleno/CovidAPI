using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class InputPOST
    {
        
        [JsonPropertyName("covid_data")]
        public CovidData oCovidData{get;set;}

        public InputPOST(){}
        public InputPOST(CovidData pCovidData)
        {
            this.oCovidData = pCovidData;
        }
    }
}