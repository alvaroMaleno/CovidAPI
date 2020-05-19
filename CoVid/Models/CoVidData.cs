namespace CoVid.Models
{
    public class CoVidData
    {
        public ulong id{get;set;}
        public int cases {get;set;}
        public int deaths {get;set;}
        public int cured {get;set;}
        public CovidDate date{get;set;}
        
    }
}