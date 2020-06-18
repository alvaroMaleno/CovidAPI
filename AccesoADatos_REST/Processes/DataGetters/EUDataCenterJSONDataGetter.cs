using System.Globalization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVid.Models;
using CoVid.Processes.DataGetters.Interfaces;
using CoVid.Utils;
using Newtonsoft.Json.Linq;

namespace CoVid.Processes.DataGetters
{
    public class EUDataCenterJSONDataGetter : IDataGetter
    {
        private readonly string _RIGHT_BAR = "/";
        private readonly string _DATE_FORMAT = "dd/MM/yyyy";
        private readonly string _URL_TO_COMPLETE_DATA = "https://api.covid19api.com/country/";
        private Records _oRecords;
        private string _url;
        
        public EUDataCenterJSONDataGetter(InitDataGetting pCallerClass)
        {
            _url = pCallerClass.url;
        }

        public void GetData(string pPath)
        {
            UtilsJSON.GetInstance().DeserializeFromUrl(out _oRecords, pPath);
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

            await this.CompleteEUData(pDicToComplete);
        
        }

        private async Task CompleteEUData(ConcurrentDictionary<string, GeoZone> pDicToComplete)
        {
            Task[] oTaskArray = new Task[100];
            bool hasBeenCompletedOneTime = false;
            int index = oTaskArray.Length - 1;

            foreach (var oGeoIdKeyGeoZoneValue in pDicToComplete)
            {
                if(hasBeenCompletedOneTime)
                {
                    await oTaskArray[index];
                    oTaskArray[index--] = this.CompleteEUDataByCountry(oGeoIdKeyGeoZoneValue.Value);
                }
                else
                {
                    oTaskArray[index--] = this.CompleteEUDataByCountry(oGeoIdKeyGeoZoneValue.Value);
                }
                if(index < 0)
                {
                    hasBeenCompletedOneTime = true;
                    index = oTaskArray.Length -1;
                }
            }
        }

        private async Task CompleteEUDataByCountry(GeoZone pGeoZone)
        {
            try
            {
                string url = string.Join(string.Empty, this._URL_TO_COMPLETE_DATA, pGeoZone.geoID);
                JArray oJArrayCountry;
                UtilsJSON.GetInstance().JsonParseJArrayFromUrl(out oJArrayCountry, url);

                if(oJArrayCountry is null)
                    return;

                Country oCountry = new Country();
                oCountry.oCountryList = new List<CountryData>();

                CountryData oCountryData;
                foreach (var oItem in oJArrayCountry)
                {
                    oCountryData = new CountryData();
                    
                    oCountryData.Country = oItem?.Value<string>("Country");
                    oCountryData.CountryCode = oItem?.Value<string>("CountryCode");
                    oCountryData.province = oItem?.Value<string>("Province");
                    oCountryData.City = oItem?.Value<string>("City");
                    oCountryData.CityCode = oItem?.Value<string>("CityCode");
                    oCountryData.Lat = oItem?.Value<string>("Lat");
                    oCountryData.Lon = oItem?.Value<string>("Lon");
                    oCountryData.Confirmed = oItem?.Value<string>("Confirmed");
                    oCountryData.Deaths = oItem?.Value<string>("Deaths");
                    oCountryData.Recovered = oItem?.Value<string>("Recovered");
                    oCountryData.Active = oItem?.Value<string>("Active");
                    oCountryData.Date = oItem?.Value<string>("Date");

                    oCountry.oCountryList.Add(oCountryData);
                }

                Dictionary<string, CountryData> oDateCountryDataDictionary = new Dictionary<string, CountryData>();

                foreach (var oCountryDataValuePair in oCountry.oCountryList)
                {
                    var oDateTime = DateTime.ParseExact(
                        oCountryDataValuePair.Date.Split(" ")[0], 
                        "MM/dd/yyyy", CultureInfo.CurrentCulture);
                    if(oDateCountryDataDictionary.ContainsKey(oDateTime.ToString(_DATE_FORMAT)))
                    {
                        continue;
                    }
                    oDateCountryDataDictionary.Add(oDateTime.ToString(_DATE_FORMAT), oCountryDataValuePair);
                }

                string yesterdayDate = null;
                foreach (var oData in pGeoZone.dataList)
                {
                    if(!oDateCountryDataDictionary.ContainsKey(oData.date.date))
                    {
                        continue;
                    }
                    
                    var f_oCountryData = oDateCountryDataDictionary[oData.date.date];
                    int cured;
                    int.TryParse(f_oCountryData.Recovered, out cured);

                    if(yesterdayDate != null && 
                    oDateCountryDataDictionary.ContainsKey(oData.date.date))
                    {
                        var f_oCountryDataYesterday = oDateCountryDataDictionary[yesterdayDate];
                        int curedYesterday;
                        int.TryParse(f_oCountryDataYesterday.Recovered, out curedYesterday);
                        oData.cured = cured - curedYesterday;
                    }
                    else
                    {
                        oData.cured = cured;
                    }
                    
                    yesterdayDate = oData.date.date;
                }
            }
            catch (Exception ex)
            {
                ex = null;
                return;
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