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

        [JsonPropertyName("dataType")]
        public string dataType{get;set;}
        
        public CovidData(){}
        public CovidData(List<string> pCountryList, Dates pDates, string pDataType)
        {
            this.oCountryList = pCountryList;
            this.oDates = pDates;
            this.dataType = pDataType;
        }
    }
}