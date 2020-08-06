using System.Text.Json.Serialization;

namespace CoVid.Models
{
    public class CovidDate
    {
        [JsonPropertyName("id")]
        public ulong id {get;set;}
        [JsonPropertyName("date")]
        public string date {get;set;}
        [JsonPropertyName("dateSeparator")]
        public string dateSeparator {get;set;}
        [JsonPropertyName("dateFormat")]
        public string dateFormat {get;set;}

        public CovidDate(){}
        public CovidDate(
            ulong pDateId, 
            string pDate, 
            string pDateSeparator, 
            string pDateFormat){
                
            this.id = pDateId;
            this.date = pDate;
            this.dateSeparator = pDateSeparator;
            this.dateFormat = pDateFormat;
        }
    }
}