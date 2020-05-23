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
        
        [JsonPropertyName("statisticalMethod")]
        public string statisticalMethod{get;set;}

        public CovidData(){}
        public CovidData(List<string> pCountryList, Dates pDates, string pDataType, string pStatisticalMethod)
        {
            this.oCountryList = pCountryList;
            this.oDates = pDates;
            this.dataType = pDataType;
            this.statisticalMethod = pStatisticalMethod;
        }
    }
}