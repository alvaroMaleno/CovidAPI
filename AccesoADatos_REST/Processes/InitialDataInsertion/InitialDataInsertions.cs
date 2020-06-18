using System.Threading;
using System.Linq;
using System.Collections.Concurrent;
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
            while (true)
            {
                _oInitDataGetting = InitDataGetting.GetInstance(_EU_URL, _EU_DATA_CENTER);
                var oGeoZonesList = _oInitDataGetting.GetGeoZones();

                _oCovidDao.InsertGeoZoneList(oGeoZonesList);
                System.GC.Collect();
                _oCovidDao.InsertGeoZoneCountryList(oGeoZonesList);
                System.GC.Collect();
                await this.InsertDateList(oGeoZonesList);
                System.GC.Collect();
                
                if(_createGeoZoneDataTables)
                {
                    if(await this.StillBeingNecessaryCreateAllTables(_oInitDataGetting.oGeoZoneDictionary))
                    {
                        this.CreateGeoZoneDataTable(_oInitDataGetting.oGeoZoneDictionary);
                    }
                    System.GC.Collect();
                }
                
                this.InsertCovidData(_oInitDataGetting.oGeoZoneDictionary);
                System.GC.Collect();
                
                _oInitDataGetting.Clean();
                System.GC.Collect();
                //Miliseconds in a day
                Thread.Sleep(24 * 60 * 60 * 1000);
            }
            
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
            
            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                this.InsertCovidDataList(oZoneCodeGeoZoneValue);
                //DataBase can fall down and experience has shown 300 ms are a good time.
                Thread.Sleep(350);
            }

            int times = 0;
            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                while(! await this.CheckTable(oZoneCodeGeoZoneValue.Key) && times != 10)
                {
                    this.InsertCovidDataList(oZoneCodeGeoZoneValue);
                    times++;
                }
                times = 0;
            }
        }

        private void InsertCovidDataList(KeyValuePair<string, GeoZone> pZoneCodeGeoZoneValue)
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