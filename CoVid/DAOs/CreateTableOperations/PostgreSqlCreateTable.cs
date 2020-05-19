using CoVid.Controllers.DAOs.Connection;
using CoVid.Models.PathModels;
using CoVid.Models.QueryModels;
using CoVid.Utils;

namespace CoVid.Controllers.DAOs.CreateTableOperations
{
    public class PostgreSqlCreateTable : ICreate<ConnectionPostgreSql>
    {
        private Query _oQuery;
        private string[] _oPathsArray;

        public PostgreSqlCreateTable()
        {
            this.SetPaths();
        }

        private void SetPaths()
        {
            string createTablePaths;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
            {
                createTablePaths = @"./Processes/InitialCreateTables/createTables_Unix_Paths.json";
            }
            else
            {
                createTablePaths = @".\Processes\InitialCreateTables\createTables_Unix_Paths.json";
            }

            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            Paths oPathsArray;
            UtilsJSON.GetInstance().DeserializeFromString(out oPathsArray, paths);
            _oPathsArray = oPathsArray.oPaths;
        }

        public bool CreateTable(ConnectionPostgreSql pConnector, string pPath)
        {
            Query oQuery;
            this.SetQuery(pPath, out oQuery);
            
            return pConnector.ExecuteCommand(this._oQuery.query);
        }

        public bool CreateNamedDataTable(
            ConnectionPostgreSql pConnector, 
            string pPath, 
            string pTableName)
        {
            Query oQuery;
            this.SetQuery(pPath, out oQuery);
            this._oQuery.query = this._oQuery.query.Replace("country_name", pTableName);
            
            return pConnector.ExecuteCommand(this._oQuery.query);
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            string path = string.Empty;
            switch (pPath.ToLower())
            {   
                case "countries":
                    path = _oPathsArray[1];
                    break;
                case "dates":
                    path = _oPathsArray[2];
                    break;
                case "name":
                    path = _oPathsArray[3];
                    break;
                case "geozone":
                    path = _oPathsArray[0];
                    break;
                
                default:
                    break;
            }
            string query = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(path);
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}