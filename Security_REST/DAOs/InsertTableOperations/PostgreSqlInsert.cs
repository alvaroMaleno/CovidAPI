using CoVid.Controllers.DAOs.Connection;
using CoVid.Models.QueryModels;
using CoVid.Models.PathModels;
using Security_REST.Utils;

namespace CoVid.DAOs.InsertTableOperations
{
    public class PostgreSqlInsert 
    {
        private readonly int _ONE = 1;
        private readonly string _REPLACE_QUERY_CONSTANT = "?";
        private readonly string _REPLACE_SINGLEQUOTE_CONSTANT = "'";
        private readonly string _THREE_STRING = "{3}";
        private readonly string _TWO_STRING = "{2}";
        private readonly string _ONE_STRING = "{1}";
        private readonly string _ZERO_STRING = "{0}";
        private readonly string _COME = ",";
        private readonly string _NULL = "null";
        private string _INSERT_DATES_QUERY_PATH;
        private string _INSERT_GEOZONE_NAME_QUERY_PATH;
        private string _INSERT_GEOZONE_QUERY_PATH;
        private string _INSERT_GEOZONE_COUNTRIES_QUERY_PATH;
        public ConnectionPostgreSql oConnectionPostgreSql { get; set; }
        private static PostgreSqlInsert _instance; 

        private PostgreSqlInsert(ConnectionPostgreSql pConnectionPostgreSql)
        {
            this.oConnectionPostgreSql = pConnectionPostgreSql;
            this.SetPaths();
        }

        public static PostgreSqlInsert GetInstance(ConnectionPostgreSql pConnectionPostgreSql)
        {
            if(_instance is null){_instance = new PostgreSqlInsert(pConnectionPostgreSql);}
            return _instance;
        }

        private void SetPaths()
        {
            string createTablePaths = string.Empty;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
            {
                createTablePaths = @"./Processes/InitialDataInsertion/insertData_Unix_Paths.json";
            }
            else
            {
                createTablePaths = @".\Processes\InitialDataInsertion\insertData_Windows_Paths.json";
            }
            
            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            Paths oPathsArray;
            UtilsJSON.GetInstance().DeserializeFromString(out oPathsArray, paths);
            this._INSERT_DATES_QUERY_PATH = oPathsArray.oPaths[0];
            this._INSERT_GEOZONE_COUNTRIES_QUERY_PATH = oPathsArray.oPaths[3];
            this._INSERT_GEOZONE_NAME_QUERY_PATH = oPathsArray.oPaths[1];
            this._INSERT_GEOZONE_QUERY_PATH = oPathsArray.oPaths[2];
        }


        public void SetQuery(string pPath, out Query pQuery)
        {
            string query = UtilsStreamReaders.GetInstance().ReadStreamFile(pPath);
            UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}