using System.Collections.Generic;
using CoVid.Models;
using CoVid.Models.InputModels;

namespace CoVid.Controllers.DAOs
{
    public interface ICovidDataBaseAccess
    {
        public List<GeoZone> GetGeoZoneData(CovidData pCovidData);
        public List<GeoZone> GetAllGeoZoneData(CovidData pCovidData);
    }
}