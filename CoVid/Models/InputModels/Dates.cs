using System.Text.Json.Serialization;

namespace CoVid.Models.InputModels
{
    public class Dates
    {
        [JsonPropertyName("startDate")]
        public string startDate{get;set;}
        [JsonPropertyName("endDate")]
        public string endDate{get;set;}

        public Dates(){}
        public Dates(string pStartDate, string pEndDate)
        {
            this.startDate = pStartDate;
            this.endDate = pEndDate;
        }
    }
}