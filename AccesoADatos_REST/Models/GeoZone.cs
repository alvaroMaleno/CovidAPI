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
        public int? population {get;set;}
        public ConcurrentBag<CoVidData> dataList {get;set;}

        public GeoZone()
        {
        }

        public GeoZone(GeoZone pGeoZone, bool copyDataList = true)
        {
            if(pGeoZone.father != null)
                this.father = new GeoZone(pGeoZone.father);
            if(pGeoZone.sonList != null)
                this.sonList = new ConcurrentBag<GeoZone>(pGeoZone.sonList);
            
            this.geoID = pGeoZone.geoID;
            this.code = pGeoZone.code;
            this.name = pGeoZone.name;
            this.population = pGeoZone.population;
            
            if(copyDataList)
                this.dataList = new ConcurrentBag<CoVidData>(pGeoZone.dataList);
        }

        public int CompareTo(object obj)
        {
            GeoZone b = (GeoZone) obj;
            return this.geoID.CompareTo(b.geoID);
        }
    }
}