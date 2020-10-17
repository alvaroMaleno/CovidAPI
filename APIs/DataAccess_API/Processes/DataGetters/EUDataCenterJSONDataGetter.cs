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
using Covid_REST.Utils;

namespace CoVid.Processes.DataGetters
{
    public class EUDataCenterJSONDataGetter : IDataGetter
    {
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
            int dataId = UtilsConstants.IntConstants.ZERO;
            foreach (var oItem in _oRecords._oRecordList){
                oTaskList.Add(this.AddItem(oItem, pDicToComplete, dataId++));
            }
            
            this._oRecords = null;
            
            foreach (var oTask in oTaskList)
                await oTask;
            
            var oDateList = await this.GetCovidDateList(this.GetGeoZones(pDicToComplete));
            this.CompleteDicData(oDateList, pDicToComplete);
            await this.CompleteEUData(pDicToComplete);
        }

        private void CompleteDicData(List<CovidDate> oDateList, ConcurrentDictionary<string, GeoZone> pDicToComplete)
        {
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

        private async Task CompleteEUData(ConcurrentDictionary<string, GeoZone> pDicToComplete)
        {
            Task[] oTaskArray = new Task[UtilsConstants.PararellConstants.MAX_NUMBER_OF_TASKS];
            bool hasBeenCompletedOneTime = false;
            int index = oTaskArray.Length - UtilsConstants.IntConstants.ONE;

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
                if(index < UtilsConstants.IntConstants.ZERO)
                {
                    hasBeenCompletedOneTime = true;
                    index = oTaskArray.Length - UtilsConstants.IntConstants.ONE;
                }
            }
        }

        private async Task CompleteEUDataByCountry(GeoZone pGeoZone)
        {
            try
            {
                string url = string.Join(string.Empty, UtilsConstants.UrlConstants.URL_TO_COMPLETE_DATA, pGeoZone.geoID);
                JArray oJArrayCountry;
                UtilsJSON.GetInstance().JsonParseJArrayFromUrl(out oJArrayCountry, url);

                if(oJArrayCountry is null)
                    return;

                Country oCountry = new Country();
                oCountry.oCountryList = new List<CountryData>();
                this.AddCountryDataToCountryList(oCountry.oCountryList, oJArrayCountry);

                Dictionary<string, CountryData> oDateCountryDataDictionary = new Dictionary<string, CountryData>();
                this.FillDateCountryDataDictionary(oCountry.oCountryList, oDateCountryDataDictionary);
                
                this.CalculateDiaryDeaths(pGeoZone.dataList, oDateCountryDataDictionary);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void CalculateDiaryDeaths(ConcurrentBag<CoVidData> pDataList, Dictionary<string, CountryData> pDateCountryDataDictionary)
        {
            string yesterdayDate = null;
            foreach (var oData in pDataList)
            {
                if(!pDateCountryDataDictionary.ContainsKey(oData.date.date))
                    continue;
                
                var f_oCountryData = pDateCountryDataDictionary[oData.date.date];
                int cured;
                int.TryParse(f_oCountryData.Recovered, out cured);

                if(yesterdayDate != null && 
                pDateCountryDataDictionary.ContainsKey(oData.date.date))
                {
                    var f_oCountryDataYesterday = pDateCountryDataDictionary[yesterdayDate];
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

        private void FillDateCountryDataDictionary(List<CountryData> pCountryList, Dictionary<string, CountryData> pDateCountryDataDictionary)
        {
            foreach (var oCountryDataValuePair in pCountryList)
            {
                var oDateTime = DateTime.ParseExact(
                    oCountryDataValuePair.Date.Split(" ")[UtilsConstants.IntConstants.ZERO], 
                    "MM/dd/yyyy", CultureInfo.CurrentCulture);
                
                if(pDateCountryDataDictionary.ContainsKey(oDateTime.ToString(UtilsConstants.DateConstants.API_DATE_FORMAT)))
                    continue;
                
                pDateCountryDataDictionary.Add(oDateTime.ToString(UtilsConstants.DateConstants.API_DATE_FORMAT), oCountryDataValuePair);
            }
        }

        private void AddCountryDataToCountryList(List<CountryData> pCountryList, JArray pJArrayCountry)
        {
            CountryData oCountryData;
            foreach (var oItem in pJArrayCountry)
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

                pCountryList.Add(oCountryData);
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

            var oOrderedDateList = oDateKeyCovidDateValue.Keys.ToList().OrderBy(date => DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
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

        private async Task AddItem(Record pItem, ConcurrentDictionary<string, GeoZone> pDicToComplete, int pDataId){

            GeoZone oGeoZone;
            CoVidData oCoviData;
            if(!pDicToComplete.ContainsKey(pItem.geoId))
            {
                oGeoZone = new GeoZone();
                oGeoZone.geoID = pItem.geoId;
                oGeoZone.code = pItem.countryterritoryCode;
                oGeoZone.dataList = new ConcurrentBag<CoVidData>();
                oGeoZone.name = pItem.countriesAndTerritories;
                oGeoZone.population = pItem.popData2019;

                pDicToComplete.GetOrAdd(pItem.geoId, oGeoZone);
            }

            oCoviData = new CoVidData();
            
            oCoviData.cases = pItem.cases;
            oCoviData.date = new CovidDate(
                ulong.Parse(pDataId.ToString()), 
                pItem.dateRep, 
                UtilsConstants.StringConstants.RIGHT_BAR, UtilsConstants.DateConstants.API_DATE_FORMAT);
            oCoviData.deaths = pItem.deaths;
            pDicToComplete[pItem.geoId].dataList.Add(oCoviData);
        }

    }
}