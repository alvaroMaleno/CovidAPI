using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class User
    {
        [JsonPropertyName("email")]
        public string id{get;set;}

        [JsonPropertyName("pass")]
        public string pass{get;set;}


        [JsonPropertyName("new")]
        public bool? newUser{get;set;}

        [JsonPropertyName("public_key")]
        public string public_key{get;set;}

        public User(){}
        public User(string pId, string pPass)
        {
            this.id = pId;
            this.pass = pPass;
        }
    }
}