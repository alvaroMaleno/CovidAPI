using System.Text.Json.Serialization;

namespace CoVid.Models
{
    public class CoVidData
    {
        [JsonPropertyName("id")]
        public ulong id{get;set;}
        [JsonPropertyName("cases")]
        public int cases {get;set;}
        [JsonPropertyName("deaths")]
        public int deaths {get;set;}
        [JsonPropertyName("cured")]
        public int cured {get;set;}
        [JsonPropertyName("date")]
        public CovidDate date{get;set;}

        public CoVidData(){}

        public CoVidData(
            ulong pId, 
            int pCases, 
            int pDeaths, 
            int pCured, 
            CovidDate pDate)
        {
            this.id = pId;
            this.cases = pCases;
            this.deaths = pDeaths;
            this.date = pDate;
        }
        
    }
}