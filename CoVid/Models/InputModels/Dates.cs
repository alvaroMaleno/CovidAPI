namespace CoVid.Models.InputModels
{
    public class Dates
    {
        public string startDate{get;set;}
        public string endDate{get;set;}

        public Dates(string pStartDate, string pEndDate)
        {
            this.startDate = pStartDate;
            this.endDate = pEndDate;
        }
    }
}