using System.Linq;
using System.Collections.Generic;
using CoVid.Controllers.DAOs;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Models;
using CoVid.Models.InputModels;
using CoVid.Models.QueryModels;
using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;

namespace CoVid.DAOs.SelectTableOperations
{
    public class PostgreSqlSelect : ICovidDataBaseAccess
    {
        private ConnectionPostgreSql _oConnection{get;set;}

        private readonly string _ONE_STRING = "{1}";
        private readonly string _ZERO_STRING = "{0}";
        private readonly string _TABLE_NAME = "table_name";
        private string _SELECT_FROM_COUNTRIES_QUERY_PATH = @"./DAOs/SelectTableOperations/selectFromCountries.json";
        private string _SELECT_FROM_COUNTRIES_ID_QUERY_PATH = @"./DAOs/SelectTableOperations/selectCountry.json";
        private string _SELECT_FROM_GEONAMEDTABLE_QUERY_PATH = @"./DAOs/SelectTableOperations/selectFromGeoNameTableBetweenDates.json";
        private string _SELECT_ID_QUERY_PATH = @"./DAOs/SelectTableOperations/selectIDFromDates.json";
        private string _SELECT_DATES_QUERY_PATH = @"./DAOs/SelectTableOperations/selectAllDates.json";

        private static PostgreSqlSelect _instance;

        private PostgreSqlSelect(ConnectionPostgreSql pConnection = null)
        {
            if(pConnection is null)
                this._oConnection = new ConnectionPostgreSql();
            else
                this._oConnection = pConnection;
        }

        public static PostgreSqlSelect GetInstance(ConnectionPostgreSql pConnection = null)
        {
            if(_instance is null)
            {
                _instance = new PostgreSqlSelect(pConnection);
            }
            return _instance;
        }

        public void GetGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            Tuple<string, string> oStartIDEndID;
            this.GetDatesIDs(pCovidData, out oStartIDEndID);

            if(oStartIDEndID is null){return;}
            
            List<GeoZone> oListToOrderComplete = new List<GeoZone>();
            this.GetGetGeoZoneListData(oStartIDEndID, pCovidData, oListToOrderComplete);
            
            pListToComplete.AddRange(
                oListToOrderComplete.OrderBy(oGeozone => oGeozone.geoID).ToList());
            
        }

        private async void GetGetGeoZoneListData(
            Tuple<string, string> pStartIDEndID, 
            CovidData pCovidData, 
            List<GeoZone> pListToComplete)
        {
            ConcurrentBag<GeoZone> oConcurrentBagToFill = new ConcurrentBag<GeoZone>();
            List<Task> oTaskList = new List<Task>();

            foreach (var country in pCovidData.oCountryList)
            {
                oTaskList.Add(this.FillCountryData(oConcurrentBagToFill, country, pStartIDEndID));
            }
            foreach (var oTask in oTaskList)
            {
                await oTask;
            }
            pListToComplete.AddRange(oConcurrentBagToFill);
        }

        private async Task FillCountryData(
            ConcurrentBag<GeoZone> oConcurrentBagToFill, 
            string country,
             Tuple<string, string> pStartIDEndID)
        {
            Query oQuery;
                List<object[]> oResultList;
                List<object[]> oCountryInfoList;
                string countryToUpper = country.ToUpper();

                oResultList = new List<object[]>();
                oCountryInfoList = new List<object[]>();
                this.SetQuery(_SELECT_FROM_COUNTRIES_ID_QUERY_PATH, out oQuery);
                oQuery.query = oQuery.query.Replace(_ZERO_STRING, countryToUpper);
                this._oConnection.ExecuteSelectCommand(oQuery.query, oCountryInfoList);

                if(oCountryInfoList.Count == 0)
                {
                    return;
                }

                this.SetQuery(_SELECT_FROM_GEONAMEDTABLE_QUERY_PATH, out oQuery);
                oQuery.query = oQuery.query.Replace(_ZERO_STRING, pStartIDEndID.Item1);
                oQuery.query = oQuery.query.Replace(_ONE_STRING, pStartIDEndID.Item2);
                oQuery.query = oQuery.query.Replace(_TABLE_NAME, countryToUpper);
                this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);

                if(oResultList.Count == 0)
                {
                    return;
                }
                this.GenerateGeoZoneData(oResultList, oCountryInfoList, oConcurrentBagToFill);
        }

        private void GenerateGeoZoneData(
            List<object[]> pResultList, 
            List<object[]> pCountryInfoList, 
            ConcurrentBag<GeoZone> pListToComplete)
        {
            var oGeoZoneRow = pCountryInfoList[0];
            GeoZone oGeoZone = new GeoZone();
            oGeoZone.geoID = oGeoZoneRow[0].ToString().TrimEnd();
            oGeoZone.code = oGeoZoneRow[1].ToString();
            oGeoZone.name = oGeoZoneRow[2].ToString();
            oGeoZone.population = int.Parse(oGeoZoneRow[3].ToString());
            oGeoZone.dataList = new ConcurrentBag<CoVidData>();

            foreach (var oRowArray in pResultList)
            {
                this.DeserializeCovidDataAndAddToConcurrentBag(oRowArray, oGeoZone.dataList);
            }
            pListToComplete.Add(oGeoZone);
        }

        private void DeserializeCovidDataAndAddToConcurrentBag(object[] oRowArray, ConcurrentBag<CoVidData> pDataList)
        {
            CoVidData oCovidData;
            var stringData = oRowArray[1]?.ToString() ?? string.Empty;
            if(stringData == string.Empty)
            {
                return;
            }
            
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out oCovidData, stringData);
            pDataList.Add(oCovidData);
        }

        private void GetDatesIDs(CovidData pCovidData, out Tuple<string, string> oStartIDEndID)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();

            this.SetQuery(_SELECT_ID_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(_ZERO_STRING, pCovidData.oDates.startDate);
            
            var isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse)
            {
                oStartIDEndID = null;
                return;
            }

            this.SetQuery(_SELECT_ID_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(_ZERO_STRING, pCovidData.oDates.endDate);
            
            isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse || oResultList.Count < 2)
            {
                oStartIDEndID = null;
                return;
            }

            var oRow = oResultList[0];
            string start = oRow[0].ToString();
            oRow = oResultList[1];
            string end = oRow[0].ToString();

            oStartIDEndID = new Tuple<string, string>(start, end);
        }

        public void GetAllGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            List<GeoZone>oGeoZoneCompleteList = new List<GeoZone>();
            this.GetAllCountries(oGeoZoneCompleteList);
            if(oGeoZoneCompleteList.Count == 0){return;}

            pCovidData.oCountryList = new List<string>();
            foreach (var oGeoZone in oGeoZoneCompleteList)
            {
                pCovidData.oCountryList.Add(oGeoZone.geoID);
            }
            this.GetGeoZoneData(pCovidData, pListToComplete);
        }

        public void GetAllDates(List<CovidDate> pCovidDateList)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();

            this.SetQuery(_SELECT_DATES_QUERY_PATH, out oQuery);
            
            var isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse || oResultList.Count == 0)
            {
                return;
            }
            
            CovidDate oCovidDate;
            int numberOfColumns = 2;
            string dateFormat = "dd/MM/yyyy";
            string dateSeparator = "/";
            foreach (var oDateRow in oResultList)
            {
                if(oDateRow.Length != numberOfColumns)
                {
                    continue;
                }
                oCovidDate = new CovidDate();
                oCovidDate.id = ulong.Parse(oDateRow[0].ToString());
                oCovidDate.date = oDateRow[1].ToString();
                oCovidDate.dateFormat = dateFormat;
                oCovidDate.dateSeparator = dateSeparator;
                pCovidDateList.Add(oCovidDate);
            }
        }

        public void GetAllCountries(List<GeoZone> pCovidCountryList)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();

            this.SetQuery(_SELECT_FROM_COUNTRIES_QUERY_PATH, out oQuery);
            
            var isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse || oResultList.Count == 0)
            {
                return;
            }
            
            GeoZone oGeoZone;
            int numberOfColumns = 4;
            foreach (var oDateRow in oResultList)
            {
                if(oDateRow.Length != numberOfColumns)
                {
                    continue;
                }
                oGeoZone = new GeoZone();
                oGeoZone.geoID = oDateRow[0].ToString();
                oGeoZone.code = oDateRow[1].ToString();
                oGeoZone.name = oDateRow[2].ToString();
                oGeoZone.population = int.Parse(oDateRow[3].ToString());
                pCovidCountryList.Add(oGeoZone);
            }
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            Utils.UtilsJSON.GetInstance().DeserializeFromString(
                out pQuery, 
                Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(pPath));
        }
    }
}