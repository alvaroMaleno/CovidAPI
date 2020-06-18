using System.Collections.Generic;
using CoVid.DAOs.Interfaces;
using CoVid.Models;
using CoVid.Models.InputModels;

namespace CoVid.Controllers.DAOs
{
    public interface ICovidDataBaseAccess:IQuery
    {
        public void GetGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete);
        public void GetAllGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete);
        public void GetAllGeoZoneDataForAllDates(List<GeoZone> pListToComplete);
        public void GetAllCountries(List<GeoZone> pCovidCountryList);
        public void GetAllDates(List<CovidDate> pCovidDateList);
    }
}