using System.Text.Json.Serialization;

namespace Security_REST.Models.DataModels
{
    public class KeyPair
    {
        [JsonPropertyName("public")]
        public string public_string{get;set;}
        
        [JsonPropertyName("private")]
        public string private_string{get;set;}

        public KeyPair(string pPublic, string pPrivate)
        {
            public_string = pPublic;
            private_string = pPrivate;
        }
    }
}