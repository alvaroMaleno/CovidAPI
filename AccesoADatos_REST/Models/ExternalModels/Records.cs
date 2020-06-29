using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoVid.Models
{
    public class Records{
        [JsonPropertyName("records")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "records")]
        public List<Record> _oRecordList{get;set;}
    }
    
    public class Record
    {
        [JsonPropertyName("dateRep")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "records")]
        public string dateRep{get;set;}
        [JsonPropertyName("day")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "day")]
        public string day{get;set;}
        [JsonPropertyName("month")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "month")]
        public string month{get;set;}
        [JsonPropertyName("year")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "year")]
        public string year{get;set;}
        [JsonPropertyName("cases")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "cases")]
        public int cases{get;set;}
        [JsonPropertyName("deaths")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "deaths")]
        public int deaths{get;set;}
        [JsonPropertyName("countriesAndTerritories")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "countriesAndTerritories")]
        public string countriesAndTerritories{get;set;}
        [JsonPropertyName("geoId")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "geoId")]
        public string geoId{get;set;}
        [JsonPropertyName("countryterritoryCode")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "countryterritoryCode")]
        public string countryterritoryCode{get;set;}
        [JsonPropertyName("popData2019")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "popData2019")]
        public int? popData2019{get;set;}
        [JsonPropertyName("continentExp")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "continentExp")]
        public string continentExp{get;set;}

    }
}