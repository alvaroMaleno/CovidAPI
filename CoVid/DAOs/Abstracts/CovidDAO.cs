using System.Collections.Generic;
using CoVid.Models;

namespace CoVid.DAOs.Abstracts
{
    public abstract class CovidDAO
    {
        public abstract bool CreateTable(string pPath);
        public abstract bool CreateNamedTable(string pPath, string pTableName);
        public abstract bool InsertGeoZone(GeoZone pGeoZone);
        public abstract bool InsertGeoZoneList(List<GeoZone> pGeoZone);
        public abstract bool InsertGeoZoneCountry(GeoZone pGeoZone);
        public abstract bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone);
        public abstract bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone);
        public abstract bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone);
        public abstract bool InsertDate(CovidDate pCovidDate);
        public abstract bool InsertDateList(List<CovidDate> pCovidDate);
    }
}