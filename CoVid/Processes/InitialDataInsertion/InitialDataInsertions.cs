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
                if(await this.StillBeingNecessaryCreateAllTables(_oInitDataGetting.oGeoZoneDictionary))
                {
                    this.CreateGeoZoneDataTable(_oInitDataGetting.oGeoZoneDictionary);
                }
            }

            this.InsertCovidData(_oInitDataGetting.oGeoZoneDictionary);
            
        }

        private async Task<bool> StillBeingNecessaryCreateAllTables(ConcurrentDictionary<string, GeoZone> pGeoZoneDictionary)
        {
            List<string> oTablesNamesList = new List<string>();

            this.FillTablesNamesList(pGeoZoneDictionary, oTablesNamesList);

            List<Task<bool>> oTaskList = new List<Task<bool>>();
            foreach (string table in oTablesNamesList)
            {
                oTaskList.Add(this.CheckTable(table));
            }
            foreach (var oTask in oTaskList)
            {
                if(await oTask == false)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> CheckTable(string pTable)
        {
            CoVid.Models.InputModels.CovidData oCovidData = new CoVid.Models.InputModels.CovidData();
            oCovidData.oDates = new Models.InputModels.Dates();
            //Sample Dates
            oCovidData.oDates.startDate = "10/05/2020";
            oCovidData.oDates.endDate = "15/05/2020";
            oCovidData.oCountryList = new List<string>(){pTable};
            
            List<GeoZone> oGeoZonesList = new List<GeoZone>();

            _oCovidDao.GetGeoZoneData(oCovidData, oGeoZonesList);

            if(oGeoZonesList.Count == 0)
            {
                return false;
            }
            return true;
        }

        private async void InsertCovidData(ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary)
        {
            //A database usually can manage about 100 connections at the same time.
            Task[] oTaskArray = new Task[90];
            bool hasBeenFinishedOneTime = false;
            int index = oTaskArray.Length - 1;
            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                if(!hasBeenFinishedOneTime)
                {
                    oTaskArray[index--] = this.InsertCovidDataList(oZoneCodeGeoZoneValue);
                }
                else
                {
                    await oTaskArray[index];
                    oTaskArray[index--] = this.InsertCovidDataList(oZoneCodeGeoZoneValue);
                }
                if(index < 0)
                {
                    hasBeenFinishedOneTime = true;
                    index = oTaskArray.Length - 1;
                }
            }
        }

        private async Task InsertCovidDataList(KeyValuePair<string, GeoZone> pZoneCodeGeoZoneValue)
        {
            this._oCovidDao.InsertCovidDataList(pZoneCodeGeoZoneValue.Value.dataList.ToList(), pZoneCodeGeoZoneValue.Value);
        }

        private void CreateGeoZoneDataTable(ConcurrentDictionary<string, GeoZone> pGeoZoneDictionary)
        {
            List<string> oTablesNamesList = new List<string>();
            this.FillTablesNamesList(pGeoZoneDictionary, oTablesNamesList);
            _oCovidDao.CreateNamedTable("name", oTablesNamesList.ToArray());
        }

        private void FillTablesNamesList(
            ConcurrentDictionary<string, GeoZone> pGeoZoneDictionary, 
            List<string> pTablesNamesList)
        {
            foreach (var oZoneCodeGeoZoneValue in pGeoZoneDictionary)
            {
                pTablesNamesList.Add(oZoneCodeGeoZoneValue.Value.geoID);
            }
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