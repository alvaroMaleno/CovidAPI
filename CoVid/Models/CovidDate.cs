namespace CoVid.Models
{
    public class CovidDate
    {
        public ulong id {get;set;}
        public string date {get;set;}
        public string dateSeparator {get;set;}
        public string dateFormat {get;set;}

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