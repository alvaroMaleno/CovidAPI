using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CoVid.Models;
using CoVid.Processes.DataGetters.Interfaces;
using CoVid.Utils;

namespace CoVid.Processes.DataGetters
{
    public class EUDataCenterJSONDataGetter : IDataGetter
    {
        private readonly string _RIGHT_BAR = "/";
        private readonly string _DATE_FORMAT = "dd/MM/yyyy";

        private Records _oRecords;
        private string _url;
        
        public EUDataCenterJSONDataGetter(InitDataGetting pCallerClass)
        {
            _url = pCallerClass.url;
        }

        public void GetData(string pPath)
        {
            HttpClient oHttpClient = new HttpClient();
            _oRecords = JsonSerializer.Deserialize<Records>(oHttpClient.GetAsync(pPath).Result.Content.ReadAsStringAsync().Result);
            oHttpClient = null;
        }

        public async void SetOwnDataModel(ConcurrentDictionary<string, GeoZone> pDicToComplete)
        {

            this.GetData(this._url);
            List<Task> oTaskList = new List<Task>();
            int dataId = 0;
            foreach (var oItem in _oRecords._oRecordList){
                oTaskList.Add(this.addItem(oItem, pDicToComplete, dataId++));
            }
            
            this._oRecords = null;
            
            foreach (var oTask in oTaskList)
                await oTask;
            
            var oDateList = await this.GetCovidDateList(this.GetGeoZones(pDicToComplete));

            foreach (var oGeoIdKeyGeoZoneValue in pDicToComplete)
            {
                foreach (var oData in oGeoIdKeyGeoZoneValue.Value.dataList)
                {
                    var oDateToSetID = oDateList.Find(oDate => oDate.date == oData.date.date);
                    oData.id = oDateToSetID.id;
                    oData.date.id = oDateToSetID.id;
                }
            }
        }

        public List<GeoZone> GetGeoZones(ConcurrentDictionary<string, GeoZone> pGeoZoneDictionary)
        {
            var listToReturn = new List<GeoZone>();
            foreach (var item in pGeoZoneDictionary)
            {
                listToReturn.Add(item.Value);
            }
            listToReturn.Sort();
            return listToReturn;
        }

        private async Task<List<CovidDate>> GetCovidDateList(List<GeoZone> oGeoZonesList)
        {
            ConcurrentDictionary<string, CovidDate> oDateKeyCovidDateValue = new ConcurrentDictionary<string, CovidDate>();
            
            await UtilsCovidDateManagement.GetInstance().CompleteCovidDatesDictionary(oDateKeyCovidDateValue, oGeoZonesList);

            var oOrderedDateList = oDateKeyCovidDateValue.Keys.ToList().OrderBy(date => DateTime.Parse(date)).ToList();
            List<CovidDate> oCovidDateList = new List<CovidDate>();

            ulong dateId = 1;
            foreach (var date in oOrderedDateList)
            {
                var oDate = oDateKeyCovidDateValue[date];
                oDate.id = dateId++;
                oCovidDateList.Add(oDateKeyCovidDateValue[date]);
            }

            return oCovidDateList;
        }

        private async Task addItem(Record pItem, ConcurrentDictionary<string, GeoZone> pDicToComplete, int pDataId){

            GeoZone oGeoZone;
            CoVidData oCoviData;
            if(!pDicToComplete.ContainsKey(pItem.geoId))
            {
                    
                oGeoZone = new GeoZone();
                oGeoZone.geoID = pItem.geoId;
                oGeoZone.code = pItem.countryterritoryCode;
                oGeoZone.dataList = new ConcurrentBag<CoVidData>();
                oGeoZone.name = pItem.countriesAndTerritories;
                int pop;
                int.TryParse(pItem.popData2018, out pop);
                oGeoZone.population = pop;

                pDicToComplete.GetOrAdd(pItem.geoId, oGeoZone);
            }

            oCoviData = new CoVidData();
            
            int auxiliarInt;
            int.TryParse(pItem.cases, out auxiliarInt);
            oCoviData.cases = auxiliarInt;
            oCoviData.date = new CovidDate(ulong.Parse(pDataId.ToString()), pItem.dateRep, _RIGHT_BAR, _DATE_FORMAT);
            int.TryParse(pItem.deaths, out auxiliarInt);
            oCoviData.deaths = auxiliarInt;
            pDicToComplete[pItem.geoId].dataList.Add(oCoviData);
        }

    }
}