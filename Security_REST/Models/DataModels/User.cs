using System.Text.Json.Serialization;

namespace Security_REST.Models.DataModels
{
    public class User
    {
        [JsonPropertyName("email")]
        public string email{get;set;}
        
        [JsonPropertyName("pass")]
        public string pass{get;set;}

        [JsonPropertyName("new")]
        public bool? newUser{get;set;}
    }
}