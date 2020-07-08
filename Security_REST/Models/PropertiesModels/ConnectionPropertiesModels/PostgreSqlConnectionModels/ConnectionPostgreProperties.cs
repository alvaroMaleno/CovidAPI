using System.Text.Json.Serialization;

namespace Security_REST.Controllers.DAOs.Connection
{
    public class ConnectionPostgreProperties
    {
        [JsonPropertyName("server")]
        public string server{get;set;}

        [JsonPropertyName("port")]
        public string port{get;set;}

        [JsonPropertyName("userId")]
        public string userId{get;set;}

        [JsonPropertyName("pass")]
        public string pass{get;set;}

        [JsonPropertyName("dataBase")]
        public string dataBase{get;set;}
    }
}