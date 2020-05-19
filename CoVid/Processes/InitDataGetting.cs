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
        //TODO Think about pDataGetterTypeParam
        private InitDataGetting(string pUrl, string pDataGetterType)
        {
            this.url = pUrl;
            this.oGeoZoneDictionary = new ConcurrentDictionary<string, GeoZone>();
            this.SetIDataGetter(pDataGetterType);
            _oIDataGetter.SetOwnDataModel(oGeoZoneDictionary);       
        }

        //TODO Refactor to a Factory
        private void SetIDataGetter(string pType){
            switch (pType)
            {
                case "EUDataCenterJSONDataGetter":
                    this._oIDataGetter = new EUDataCenterJSONDataGetter(this);
                    break;

            }
        }

        //TODO Implement DataBase Insertion
        public List<GeoZone> GetGeoZones()
        {
            var listToReturn = new List<GeoZone>();
            foreach (var item in this.oGeoZoneDictionary)
            {
                listToReturn.Add(item.Value);
            }
            listToReturn.Sort();
            return listToReturn;
        }

    }
}