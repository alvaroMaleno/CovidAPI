using System.Linq;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoVid.Models;
using CoVid.Processes.Interfaces;
using CoVid.DAOs.Abstracts;
using CoVid.Utils;
using CoVid.Models.PathModels;

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
                await this.CreateGeoZoneDataTable(_oInitDataGetting.oGeoZoneDictionary);
            }

            await this.InsertCovidData(_oInitDataGetting.oGeoZoneDictionary);
            
        }

        private async Task InsertCovidData(ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary)
        {
            Task[] oTaskArray = new Task[_MAX_NUMBER_OF_TASKS];
            int taskCounter = oTaskArray.Length - 1;
            bool hasBeenCompletedOneTime = false;

            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                if(!hasBeenCompletedOneTime)
                {
                    oTaskArray[taskCounter--] = this.InsertCovidDataInfo(oZoneCodeGeoZoneValue.Value);
                }
                else
                {
                    await oTaskArray[taskCounter];
                    oTaskArray[taskCounter--] = this.InsertCovidDataInfo(oZoneCodeGeoZoneValue.Value);
                }
                if(taskCounter < 0)
                {
                    hasBeenCompletedOneTime = true;
                    taskCounter = oTaskArray.Length - 1;
                }
            }
        }

        private async Task InsertCovidDataInfo(GeoZone pGeoZone)
        {
            this._oCovidDao.InsertCovidDataList(pGeoZone.dataList.ToList(), pGeoZone);
        }

        private async Task CreateGeoZoneDataTable(ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary)
        {
            Task[] oTaskArray = new Task[_MAX_NUMBER_OF_TASKS];
            int taskCounter = oTaskArray.Length - 1;
            bool hasBeenCompletedOneTime = false;

            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                if(!hasBeenCompletedOneTime)
                {
                    oTaskArray[taskCounter--] = this.CreateCountryTableIfDoesNotExists(oZoneCodeGeoZoneValue);
                }
                else
                {
                    await oTaskArray[taskCounter];
                    oTaskArray[taskCounter--] = this.CreateCountryTableIfDoesNotExists(oZoneCodeGeoZoneValue);
                }
                if(taskCounter < 0)
                {
                    hasBeenCompletedOneTime = true;
                    taskCounter = oTaskArray.Length - 1;
                }
            }
        }

        private async Task CreateCountryTableIfDoesNotExists(KeyValuePair<string, GeoZone> oZoneCodeGeoZoneValue)
        {
            _oCovidDao.CreateNamedTable("name", oZoneCodeGeoZoneValue.Value.geoID);
        }

        private async Task InsertDateList(List<GeoZone> oGeoZonesList)
        {
            ConcurrentDictionary<string, CovidDate> oDateKeyCovidDateValue = new ConcurrentDictionary<string, CovidDate>();
            
            await this.CompleteCovidDatesDictionary(oDateKeyCovidDateValue, oGeoZonesList);

            var oOrderedDateList = oDateKeyCovidDateValue.Keys.ToList().OrderBy(date => DateTime.Parse(date)).ToList();
            List<CovidDate> oCovidDateList = new List<CovidDate>();

            ulong dateId = 1;
            foreach (var date in oOrderedDateList)
            {
                var oDate = oDateKeyCovidDateValue[date];
                oDate.id = dateId++;
                oCovidDateList.Add(oDateKeyCovidDateValue[date]);
            }
                
            _oCovidDao.InsertDateList(oCovidDateList);
        }


        private async Task CompleteCovidDatesDictionary(
            ConcurrentDictionary<string, CovidDate> oDateKeyCovidDateValue, 
            List<GeoZone> oGeoZonesList)
        {
            Task[] oTaskArray = new Task[_MAX_NUMBER_OF_TASKS];
            int taskCounter = oTaskArray.Length - 1;
            bool hasBeenCompletedOneTime = false;

            foreach (var oGeoZone in oGeoZonesList)
            {
                if(!hasBeenCompletedOneTime)
                {
                    oTaskArray[taskCounter--] = this.AddDateToConcurrentDictionary(oGeoZone, oDateKeyCovidDateValue);
                }
                else
                {
                    await oTaskArray[taskCounter];
                    oTaskArray[taskCounter--] = this.AddDateToConcurrentDictionary(oGeoZone, oDateKeyCovidDateValue);
                }
                if(taskCounter < 0)
                {
                    hasBeenCompletedOneTime = true;
                    taskCounter = oTaskArray.Length - 1;
                }
            }
        }

        private async Task AddDateToConcurrentDictionary(
            GeoZone oGeoZone, 
            ConcurrentDictionary<string, CovidDate> pDateKeyCovidDateValue)
        {
            foreach (var oData in oGeoZone.dataList)
            {
                if(!pDateKeyCovidDateValue.ContainsKey(oData.date.date))
                    pDateKeyCovidDateValue.GetOrAdd(oData.date.date, oData.date);
            }
        }
    }
}