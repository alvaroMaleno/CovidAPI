using System.Text.Json.Serialization;

namespace Security_REST.Models.DataModels
{
    public class ToEncrypt
    {
        [JsonPropertyName("key")]
        public string key{get;set;}
        
        [JsonPropertyName("text")]
        public string text{get;set;}
    }
}