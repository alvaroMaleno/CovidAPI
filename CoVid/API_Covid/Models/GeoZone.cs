using System;
using System.Collections.Concurrent;

namespace CoVid.Models
{
    public class GeoZone : IComparable
    {
        public GeoZone father {get;set;}
        public ConcurrentBag<GeoZone> sonList {get;set;}
        public string geoID {get;set;}
        public string code {get;set;}
        public string name {get;set;}
        public int population {get;set;}
        public ConcurrentBag<CoVidData> dataList {get;set;}

        public int CompareTo(object obj)
        {
            GeoZone b = (GeoZone) obj;
            return this.geoID.CompareTo(b.geoID);
        }
    }
}