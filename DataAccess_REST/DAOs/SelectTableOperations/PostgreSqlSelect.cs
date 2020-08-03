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
using CoVid.Utils;
using CoVid.Models.PathModels;
using Covid_REST.Utils;

namespace CoVid.DAOs.SelectTableOperations
{
    public class PostgreSqlSelect : ICovidDataBaseAccess
    {
        private ConnectionPostgreSql _oConnection{get;set;}

        private string _SELECT_FROM_COUNTRIES_QUERY_PATH;
        private string _SELECT_FROM_COUNTRIES_ID_QUERY_PATH;
        private string _SELECT_FROM_GEONAMEDTABLE_QUERY_PATH;
        private string _SELECT_ID_QUERY_PATH;
        private string _SELECT_DATES_QUERY_PATH;

        private static PostgreSqlSelect _instance;

        private PostgreSqlSelect(ConnectionPostgreSql pConnection = null)
        {
            if(pConnection is null)
                this._oConnection = new ConnectionPostgreSql();
            else
                this._oConnection = pConnection;

            this.SetPaths();
        }

        public static PostgreSqlSelect GetInstance(ConnectionPostgreSql pConnection = null)
        {
            if(_instance is null)
            {
                _instance = new PostgreSqlSelect(pConnection);
            }
            return _instance;
        }

        private void SetPaths()
        {
            string createTablePaths = string.Empty;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
                createTablePaths = @"./DAOs/SelectTableOperations/selectTables_Unix_Paths.json";
            else
                createTablePaths = @".\\DAOs\\SelectTableOperations\\selectTables_Windows_Paths.json";
            
            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            Paths oPathsArray;
            UtilsJSON.GetInstance().DeserializeFromString(out oPathsArray, paths);
            this._SELECT_FROM_COUNTRIES_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.ZERO];
            this._SELECT_FROM_COUNTRIES_ID_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.ONE];
            this._SELECT_FROM_GEONAMEDTABLE_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.TWO];
            this._SELECT_ID_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.THREE];
            this._SELECT_DATES_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.FOUR];
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
                oTaskList.Add(this.FillCountryData(
                    oConcurrentBagToFill, 
                    country,
                     pStartIDEndID.Item1, 
                     pStartIDEndID.Item2));
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
            string pStartDate,
            string pEndDate)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();
            List<object[]> oCountryInfoList = new List<object[]>();
            string countryToUpper = country.ToUpper();

            this.SetQuery(_SELECT_FROM_COUNTRIES_ID_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.ZERO_STRING, countryToUpper);
            this._oConnection.ExecuteSelectCommand(oQuery.query, oCountryInfoList);

            if(oCountryInfoList.Count == UtilsConstants.IntConstants.ZERO)
                return;

            this.SetQuery(_SELECT_FROM_GEONAMEDTABLE_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.ZERO_STRING, pStartDate);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.ONE_STRING, pEndDate);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.TABLE_NAME, countryToUpper);
            this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);

            if(oResultList.Count == UtilsConstants.IntConstants.ZERO)
                return;
            
            this.GenerateGeoZoneData(oResultList, oCountryInfoList, oConcurrentBagToFill);
        }

        private void GenerateGeoZoneData(
            List<object[]> pResultList, 
            List<object[]> pCountryInfoList, 
            ConcurrentBag<GeoZone> pListToComplete)
        {
            var oGeoZoneRow = pCountryInfoList[UtilsConstants.IntConstants.ZERO];
            GeoZone oGeoZone = new GeoZone();
            oGeoZone.geoID = oGeoZoneRow[UtilsConstants.IntConstants.ZERO].ToString().TrimEnd();
            oGeoZone.code = oGeoZoneRow[UtilsConstants.IntConstants.ONE].ToString();
            oGeoZone.name = oGeoZoneRow[UtilsConstants.IntConstants.TWO].ToString();
            oGeoZone.population = int.Parse(oGeoZoneRow[3].ToString());
            oGeoZone.dataList = new ConcurrentBag<CoVidData>();

            foreach (var oRowArray in pResultList)
                this.DeserializeCovidDataAndAddToConcurrentBag(oRowArray, oGeoZone.dataList);
            
            pListToComplete.Add(oGeoZone);
        }

        private void DeserializeCovidDataAndAddToConcurrentBag(object[] oRowArray, ConcurrentBag<CoVidData> pDataList)
        {
            CoVidData oCovidData;
            var stringData = oRowArray[UtilsConstants.IntConstants.ONE]?.ToString() ?? string.Empty;
            if(stringData == string.Empty)
                return;
            
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out oCovidData, stringData);
            pDataList.Add(oCovidData);
        }

        public void GetDatesIDs(CovidData pCovidData, out Tuple<string, string> oStartIDEndID)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();

            this.SetQuery(_SELECT_ID_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.ZERO_STRING, pCovidData.oDates.startDate);
            
            var isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse)
            {
                oStartIDEndID = null;
                return;
            }

            this.SetQuery(_SELECT_ID_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.ZERO_STRING, pCovidData.oDates.endDate);
            
            isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse || oResultList.Count < UtilsConstants.IntConstants.TWO)
            {
                oStartIDEndID = null;
                return;
            }

            var oRow = oResultList[UtilsConstants.IntConstants.ZERO];
            string start = oRow[UtilsConstants.IntConstants.ZERO].ToString();
            oRow = oResultList[UtilsConstants.IntConstants.ONE];
            string end = oRow[UtilsConstants.IntConstants.ZERO].ToString();

            oStartIDEndID = new Tuple<string, string>(start, end);
        }

        public void GetAllGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            List<GeoZone>oGeoZoneCompleteList = new List<GeoZone>();
            this.GetAllCountries(oGeoZoneCompleteList);

            if(oGeoZoneCompleteList.Count == UtilsConstants.IntConstants.ZERO){return;}

            pCovidData.oCountryList = new List<string>();
            foreach (var oGeoZone in oGeoZoneCompleteList)
                pCovidData.oCountryList.Add(oGeoZone.geoID);
            
            this.GetGeoZoneData(pCovidData, pListToComplete);
        }

        public void GetAllGeoZoneDataForAllDates(List<GeoZone> pListToComplete)
        {
            List<GeoZone>oGeoZoneCompleteList = new List<GeoZone>();
            this.GetAllCountries(oGeoZoneCompleteList);
            
            if(oGeoZoneCompleteList.Count == UtilsConstants.IntConstants.ZERO){return;}

            List<CovidDate> oCovidDateCompleteList = new List<CovidDate>();
            this.GetAllDates(oCovidDateCompleteList);
            
            if(oCovidDateCompleteList.Count == UtilsConstants.IntConstants.ZERO){return;}

            CovidData oCovidData = new CovidData();
            oCovidData.oDates = new Dates();

            oCovidData.oDates.startDate = oCovidDateCompleteList[UtilsConstants.IntConstants.ZERO].date;
            oCovidData.oDates.endDate = 
                oCovidDateCompleteList[oCovidDateCompleteList.Count - UtilsConstants.IntConstants.ONE].date;

            oCovidData.oCountryList = new List<string>();
            foreach (var oGeoZone in oGeoZoneCompleteList)
                oCovidData.oCountryList.Add(oGeoZone.geoID);
            
            this.GetGeoZoneData(oCovidData, pListToComplete);
        }

        public void GetAllDates(List<CovidDate> pCovidDateList)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();

            this.SetQuery(_SELECT_DATES_QUERY_PATH, out oQuery);
            
            var isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse || oResultList.Count == UtilsConstants.IntConstants.ZERO)
                return;
            
            CovidDate oCovidDate;
            int numberOfColumns = UtilsConstants.IntConstants.TWO;
            foreach (var oDateRow in oResultList)
            {
                if(oDateRow.Length != numberOfColumns)
                    continue;
                
                oCovidDate = new CovidDate();
                oCovidDate.id = ulong.Parse(oDateRow[UtilsConstants.IntConstants.ZERO].ToString());
                oCovidDate.date = oDateRow[UtilsConstants.IntConstants.ONE].ToString();
                oCovidDate.dateFormat = UtilsConstants.DateConstants.API_DATE_FORMAT;
                oCovidDate.dateSeparator = UtilsConstants.StringConstants.RIGHT_BAR;
                pCovidDateList.Add(oCovidDate);
            }
        }

        public void GetAllCountries(List<GeoZone> pCovidCountryList)
        {
            Query oQuery;
            List<object[]> oResultList = new List<object[]>();
            this.SetQuery(_SELECT_FROM_COUNTRIES_QUERY_PATH, out oQuery);
            
            var isAWellResponse = this._oConnection.ExecuteSelectCommand(oQuery.query, oResultList);
            if(!isAWellResponse || oResultList.Count == UtilsConstants.IntConstants.ZERO)
                return;
            
            GeoZone oGeoZone;
            int numberOfColumns = UtilsConstants.IntConstants.FOUR;
            foreach (var oDateRow in oResultList)
            {
                if(oDateRow.Length != numberOfColumns)
                    continue;
                
                oGeoZone = new GeoZone();
                oGeoZone.geoID = oDateRow[UtilsConstants.IntConstants.ZERO].ToString().TrimEnd();
                oGeoZone.code = oDateRow[UtilsConstants.IntConstants.ONE].ToString();
                oGeoZone.name = oDateRow[UtilsConstants.IntConstants.TWO].ToString();
                oGeoZone.population = int.Parse(oDateRow[UtilsConstants.IntConstants.THREE].ToString());
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