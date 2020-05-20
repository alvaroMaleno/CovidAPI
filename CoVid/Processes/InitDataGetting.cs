using System.Collections.Generic;
using CoVid.Models;
using System.Collections.Concurrent;
using CoVid.Processes.DataGetters.Interfaces;
using CoVid.Processes.DataGetters;

namespace CoVid.Processes
{
    public class InitDataGetting
    {
        public string url {get;set;}
        public ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary {get;set;}

        private IDataGetter _oIDataGetter;

        private static InitDataGetting _instance;

        public static InitDataGetting GetInstance(string pUrl, string pDataGetterType){
            if(_instance == null){
                _instance = new InitDataGetting(pUrl, pDataGetterType);
            }
            return _instance;
        }
        
        private InitDataGetting(string pUrl, string pDataGetterType)
        {
            this.url = pUrl;
            this.oGeoZoneDictionary = new ConcurrentDictionary<string, GeoZone>();
            this.SetIDataGetter(pDataGetterType);
            _oIDataGetter.SetOwnDataModel(oGeoZoneDictionary);       
        }

        private void SetIDataGetter(string pType){
            switch (pType)
            {
                case "EUDataCenterJSONDataGetter":
                    this._oIDataGetter = new EUDataCenterJSONDataGetter(this);
                    break;

            }
        }

        public List<GeoZone> GetGeoZones()
        {
            return this._oIDataGetter.GetGeoZones(this.oGeoZoneDictionary);
        }

    }
}