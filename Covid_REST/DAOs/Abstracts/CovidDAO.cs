using System.Collections.Generic;
using CoVid.Models;
using CoVid.Models.InputModels;
using CoVid.Models.QueryModels;

namespace CoVid.DAOs.Abstracts
{
    public abstract class CovidDAO
    {
        public abstract bool CreateTable(Query pQuery);
        public abstract bool CreateTable(string pPath);
        public abstract bool CreateNamedTable(string pPath, params string[] pTableName);
        public abstract bool InsertGeoZone(GeoZone pGeoZone);
        public abstract bool InsertGeoZoneList(List<GeoZone> pGeoZone);
        public abstract bool InsertGeoZoneCountry(GeoZone pGeoZone);
        public abstract bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone);
        public abstract bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone);
        public abstract bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone);
        public abstract bool InsertDate(CovidDate pCovidDate);
        public abstract bool InsertDateList(List<CovidDate> pCovidDate);
        public abstract void GetGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete);
        public abstract void GetAllGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete);
        public abstract void GetAllCountries(List<GeoZone> pCovidCountryList);
        public abstract void GetAllDates(List<CovidDate> pCovidDateList);
    }
}