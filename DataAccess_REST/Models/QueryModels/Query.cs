using System.Text.Json.Serialization;

namespace CoVid.Models.QueryModels
{
    public class Query
    {
        [JsonPropertyName("query")]
        public string query{get;set;}

        [JsonPropertyName("valuesFormat")]
        public string valuesFormat{get;set;}
    }
}