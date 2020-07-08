using System.Collections.Generic;
using Security_REST.Models.PathModels;
using Security_REST.Models.QueryModels;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.Utils;

namespace Security_REST.Controllers.DAOs.CreateTableOperations
{
    public class PostgreSqlCreateTable : ICreate<ConnectionPostgreSql>
    {
        private readonly string _TABLE_NAME = "table_name";
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
                createTablePaths = @".\Processes\InitialCreateTables\createTables_Windows_Paths.json";
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
                oSentenceList.Add(oQuery.query.Replace(_TABLE_NAME, tableName));
            }
            return pConnector.ExecuteCommand(oSentenceList.ToArray());
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            string path = string.Empty;
            switch (pPath.ToLower())
            {   
                case "users":
                    path = _oPathsArray[0];
                    break;
                case "keys":
                    path = _oPathsArray[1];
                    break;
                default:
                    break;
            }
            string query = UtilsStreamReaders.GetInstance().ReadStreamFile(path);
            UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}