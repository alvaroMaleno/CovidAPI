using System.Threading;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoVid.Models;
using CoVid.Processes.Interfaces;
using CoVid.DAOs.Abstracts;
using CoVid.Utils;
using Covid_REST.Utils;

namespace CoVid.Processes.InitialDataInsertion
{
    public class InitialDataInsertions: ITaskable
    {
        private bool _createGeoZoneDataTables;
        private readonly string _EU_DATA_CENTER = "EUDataCenterJSONDataGetter";
        
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
                _oInitDataGetting = InitDataGetting.GetInstance(UtilsConstants.UrlConstants.EU_URL, _EU_DATA_CENTER);
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
                        this.CreateGeoZoneDataTable(_oInitDataGetting.oGeoZoneDictionary);
                    
                    System.GC.Collect();
                }
                
                this.InsertCovidData(_oInitDataGetting.oGeoZoneDictionary);
                _oInitDataGetting.Clean();
                System.GC.Collect();
                Thread.Sleep(UtilsConstants.IntConstants.MS_IN_A_DAY);
            }
            
        }

        private async Task<bool> StillBeingNecessaryCreateAllTables(ConcurrentDictionary<string, GeoZone> pGeoZoneDictionary)
        {
            List<string> oTablesNamesList = new List<string>();
            this.FillTablesNamesList(pGeoZoneDictionary, oTablesNamesList);

            List<Task<bool>> oTaskList = new List<Task<bool>>();
            foreach (string table in oTablesNamesList)
                oTaskList.Add(this.CheckTable(table));
            
            foreach (var oTask in oTaskList)
                if(await oTask == false)
                    return true;
            
            return false;
        }

        private async Task<bool> CheckTable(string pTable)
        {
            CoVid.Models.InputModels.CovidData oCovidData = new CoVid.Models.InputModels.CovidData();
            oCovidData.oDates = new Models.InputModels.Dates();
            //Sample Dates
            oCovidData.oDates.startDate = "10/06/2020";
            oCovidData.oDates.endDate = "15/06/2020";
            oCovidData.oCountryList = new List<string>(){pTable};
            
            List<GeoZone> oGeoZonesList = new List<GeoZone>();

            _oCovidDao.GetGeoZoneData(oCovidData, oGeoZonesList);

            if(oGeoZonesList.Count == UtilsConstants.IntConstants.ZERO)
                return false;
            
            return true;
        }

        private async void InsertCovidData(ConcurrentDictionary<string, GeoZone> oGeoZoneDictionary)
        {
            int MAX_NUMBER_OF_RETRIEVES = 10;

            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                this.InsertCovidDataList(oZoneCodeGeoZoneValue);
                //DataBase can fall down and experience has shown 200 ms are a good time.
                Thread.Sleep(200);
            }

            int times = UtilsConstants.IntConstants.ZERO;
            foreach (var oZoneCodeGeoZoneValue in oGeoZoneDictionary)
            {
                while(!await this.CheckTable(oZoneCodeGeoZoneValue.Key) && times != MAX_NUMBER_OF_RETRIEVES)
                {
                    this.InsertCovidDataList(oZoneCodeGeoZoneValue);
                    times++;
                }
                times = UtilsConstants.IntConstants.ZERO;
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
                pTablesNamesList.Add(oZoneCodeGeoZoneValue.Value.geoID);
        }

        private async Task InsertDateList(List<GeoZone> oGeoZonesList)
        {
            ConcurrentDictionary<string, CovidDate> oDateKeyCovidDateValue = new ConcurrentDictionary<string, CovidDate>();
            
            await UtilsCovidDateManagement.GetInstance().CompleteCovidDatesDictionary(oDateKeyCovidDateValue, oGeoZonesList);

            List<CovidDate> oCovidDateList = new List<CovidDate>();
            foreach (var oDateKeyValue in oDateKeyCovidDateValue)
                oCovidDateList.Add(oDateKeyValue.Value);
                
            _oCovidDao.InsertDateList(oCovidDateList);
        }

    }
}