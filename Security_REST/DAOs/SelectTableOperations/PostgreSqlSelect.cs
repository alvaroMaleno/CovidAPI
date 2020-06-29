using CoVid.Controllers.DAOs.Connection;
using CoVid.Models.QueryModels;

namespace CoVid.DAOs.SelectTableOperations
{
    public class PostgreSqlSelect 
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

        public void SetQuery(string pPath, out Query pQuery)
        {
            Utils.UtilsJSON.GetInstance().DeserializeFromString(
                out pQuery, 
                Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(pPath));
        }
    }
}