using System.Collections.Generic;
using Security_REST.Models.PathModels;
using Security_REST.Models.QueryModels;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.Utils;

namespace Security_REST.Controllers.DAOs.CreateTableOperations
{
    public class PostgreSqlCreateTable : ICreate<ConnectionPostgreSql>
    {
        private Paths _oPaths;

        public PostgreSqlCreateTable()
        {
            this.SetPaths();
        }

        private void SetPaths()
        {
            string createTablePaths;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
                createTablePaths = @"./DAOs/PathsFiles/createTables_Unix_Paths.json";
            else
                createTablePaths = @".\DAOs\PathsFiles\createTables_Windows_Paths.json";

            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            UtilsJSON.GetInstance().DeserializeFromString(out _oPaths, paths);
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
                oSentenceList.Add(oQuery.query.Replace(UtilsConstants._TABLE_NAME, tableName));
            }
            return pConnector.ExecuteCommand(oSentenceList.ToArray());
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            string query = UtilsStreamReaders.GetInstance().ReadStreamFile(_oPaths.oPaths[0]);
            UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}