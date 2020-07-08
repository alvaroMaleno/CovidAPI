using System.Text.Json.Serialization;

namespace Security_REST.Models.PathModels
{
    public class Paths
    {
        [JsonPropertyName("create_table_relatives_paths")]
        public string[] oPaths{get;set;}
    }
}