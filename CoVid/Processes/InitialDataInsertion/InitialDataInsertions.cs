using System.Threading;
using System.Linq;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoVid.Models;
using CoVid.Processes.Interfaces;
using CoVid.DAOs.Abstracts;
using CoVid.Utils;

namespace CoVid.Processes.InitialDataInsertion
{
    public class InitialDataInsertions: ITaskable
    {
        private readonly int _MAX_NUMBER_OF_TASKS = 200;
        private bool _createGeoZoneDataTables;
        private readonly string _EU_DATA_CENTER = "EUDataCenterJSONDataGetter";
        private static readonly string _EU_URL = "https://opendata.ecdc.europa.eu/covid19/casedistribution/json/";
        private InitDataGetting _oInitDataGetting;
        private CovidDAO _oCovidDao;

        public InitialDataInsertions(
            CovidDAO pCovidDAO, 
            bool pCreateGeoZoneDataTables = false) 
        {
            this._oCovidDao = pCovidDAO;
            this._createGeoZoneDataTables = pCreateGeoZoneDataTables;
        }

        public async Task Taskable()
        {
            _oInitDataGetting = InitDataGetting.GetInstance(_EU_URL, _EU_DATA_CENTER);
            var oGeoZonesList = _oInitDataGetting.GetGeoZones();

            _oCovidDao.InsertGeoZoneList(oGeoZonesList);
            _oCovidDao.InsertGeoZoneCountryList(oGeoZonesList);
            await this.InsertDateList(oGeoZonesList);
            
            if(_createGeoZoneDataTables)
            {
                this.CreateGeoZoneDataTable(_oInitDataGetting.oGeoZoneDictionary);
            }

            this.InsertCovidData(_oInitDataGetting.oGeoZoneDictionary);
            
            
        }

        private void InsertCovidData(ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary)
        {
            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                this._oCovidDao.InsertCovidDataList(oZoneCodeGeoZoneValue.Value.dataList.ToList(), oZoneCodeGeoZoneValue.Value);
            }
        }

        private void CreateGeoZoneDataTable(ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary)
        {
            List<string> oTablesNamesList = new List<string>();
            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                oTablesNamesList.Add(oZoneCodeGeoZoneValue.Value.geoID);
            }
            _oCovidDao.CreateNamedTable("name", oTablesNamesList.ToArray());
        }

        private async Task InsertDateList(List<GeoZone> oGeoZonesList)
        {
            ConcurrentDictionary<string, CovidDate> oDateKeyCovidDateValue = new ConcurrentDictionary<string, CovidDate>();
            
            await UtilsCovidDateManagement.GetInstance().CompleteCovidDatesDictionary(oDateKeyCovidDateValue, oGeoZonesList);

            List<CovidDate> oCovidDateList = new List<CovidDate>();
            foreach (var oDateKeyValue in oDateKeyCovidDateValue)
            {
                oCovidDateList.Add(oDateKeyValue.Value);
            }
                
            _oCovidDao.InsertDateList(oCovidDateList);
        }

    }
}