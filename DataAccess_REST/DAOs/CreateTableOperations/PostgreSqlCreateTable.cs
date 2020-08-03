using System.Collections.Generic;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Models.PathModels;
using CoVid.Models.QueryModels;
using CoVid.Utils;
using Covid_REST.Utils;

namespace CoVid.Controllers.DAOs.CreateTableOperations
{
    public class PostgreSqlCreateTable : ICreate<ConnectionPostgreSql>
    {
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
                
                createTablePaths = @"./DAOs/CreateTableOperations/createTables_Unix_Paths.json";
            }
            else
            {
                createTablePaths = @".\DAOs\CreateTableOperations\createTables_Unix_Paths.json";
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
            
            return pConnector.ExecuteCommand(oQuery.query);
        }
        public bool CreateTable(ConnectionPostgreSql pConnector, Query oQuery)
        {
            return pConnector.ExecuteCommand(oQuery.query);
        }

        public bool CreateNamedDataTable(
            ConnectionPostgreSql pConnector, 
            string pPath, 
            params string[] pTableName)
        {
            List<string> oSentenceList = new List<string>();
            foreach (var tableName in pTableName)
            {
                Query oQuery;
                this.SetQuery(pPath, out oQuery);
                oSentenceList.Add(oQuery.query.Replace(UtilsConstants.QueryConstants.COUNTRY_NAME, tableName));
            }
            return pConnector.ExecuteCommand(oSentenceList.ToArray());
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            string path = string.Empty;
            switch (pPath.ToLower())
            {   
                case "countries":
                    path = _oPathsArray[UtilsConstants.IntConstants.ONE];
                    break;
                case "dates":
                    path = _oPathsArray[UtilsConstants.IntConstants.TWO];
                    break;
                case "name":
                    path = _oPathsArray[UtilsConstants.IntConstants.THREE];
                    break;
                case "geozone":
                    path = _oPathsArray[UtilsConstants.IntConstants.ZERO];
                    break;
                case "users":
                    path = _oPathsArray[UtilsConstants.IntConstants.FOUR];
                    break;
                case "keys":
                    path = _oPathsArray[5];
                    break;
                
                default:
                    break;
            }
            string query = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(path);
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}