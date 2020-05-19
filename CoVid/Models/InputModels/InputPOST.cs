using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class InputPOST
    {
        [JsonPropertyName("user")]
        public User oUser{get;set;}
        
        [JsonPropertyName("covid_data")]
        public CovidData oCovidData{get;set;}

        public InputPOST(User pUser, CovidData pCovidData)
        {
            this.oUser = pUser;
            this.oCovidData = pCovidData;
        }
    }
}