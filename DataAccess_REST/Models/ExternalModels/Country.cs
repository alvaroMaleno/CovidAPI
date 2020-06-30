using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoVid.Models
{
    public class Country{
        public List<CountryData> oCountryList{get;set;}
    }

    public class CountryData
    {
        [JsonPropertyName("Country")]
        public string Country {get; set;}

        [JsonPropertyName("CountryCode")]
        public string CountryCode{get;set;}

        [JsonPropertyName("Province")]
        public string province{get;set;}

        [JsonPropertyName("City")]
        public string City{get;set;}

        [JsonPropertyName("CityCode")]
        public string CityCode{get;set;}

        [JsonPropertyName("Lat")]
        public string Lat{get;set;}

        [JsonPropertyName("Lon")]
        public string Lon{get;set;}

        [JsonPropertyName("Confirmed")]
        public string Confirmed{get;set;}

        [JsonPropertyName("Deaths")]
        public string Deaths{get;set;}

        [JsonPropertyName("Recovered")]
        public string Recovered{get;set;}

        [JsonPropertyName("Active")]
        public string Active{get;set;}

        [JsonPropertyName("Date")]
        public string Date{get;set;}

    }
}