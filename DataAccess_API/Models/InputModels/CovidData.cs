using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class CovidData
    {
        [JsonPropertyName("countries")]
        public List<string> oCountryList{get;set;}

        [JsonPropertyName("dates")]
        public Dates oDates{get;set;}

        public CovidData(){}
        public CovidData(List<string> pCountryList, Dates pDates)
        {
            this.oCountryList = pCountryList;
            this.oDates = pDates;
        }
    }
}