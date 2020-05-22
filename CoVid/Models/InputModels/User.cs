using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class User
    {
        [JsonPropertyName("id")]
        public string id{get;set;}
        [JsonPropertyName("pass")]
        public string pass{get;set;}

        public User(){}
        public User(string pId, string pPass)
        {
            this.id = pId;
            this.pass = pPass;
        }
    }
}