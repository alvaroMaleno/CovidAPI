using System.Collections.Generic;
using CoVid.DAOs.Interfaces;
using CoVid.Models;

namespace CoVid.Controllers.DAOs
{
    public interface ICovidDataBaseInsert : IQuery
    {
        public bool InsertGeoZone(GeoZone pGeoZone);
        public bool InsertGeoZoneList(List<GeoZone> pGeoZone);
        public bool InsertGeoZoneCountry(GeoZone pGeoZone);
        public bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone);
        public bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone);
        public bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone);
        public bool InsertDate(CovidDate pCovidDate);
        public bool InsertDateList(List<CovidDate> pCovidDate);
    }
}