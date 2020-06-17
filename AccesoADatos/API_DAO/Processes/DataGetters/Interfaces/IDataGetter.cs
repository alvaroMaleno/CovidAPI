using System.Collections.Concurrent;
using System.Collections.Generic;
using CoVid.Models;

namespace CoVid.Processes.DataGetters.Interfaces
{
    public interface IDataGetter
    {
        public void GetData(string pPath);
        public void SetOwnDataModel(ConcurrentDictionary<string, GeoZone> pDicToComplete);
        public List<GeoZone> GetGeoZones(ConcurrentDictionary<string, GeoZone> pGeoZoneDictionary);
    }
}