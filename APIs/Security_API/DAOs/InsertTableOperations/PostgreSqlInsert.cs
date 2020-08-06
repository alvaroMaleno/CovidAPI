using Security_REST.Models.QueryModels;
using Security_REST.Models.PathModels;
using Security_REST.Utils;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.DAOs.Interfaces;
using Security_REST.Models.DataModels;
using System.Collections.Generic;

namespace Security_REST.DAOs.InsertTableOperations
{
    public class PostgreSqlInsert : IQuery
    {
        private readonly string _REPLACE_SINGLEQUOTE_CONSTANT = "'";
        private Paths _oPathsArray;
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
                createTablePaths = @"./DAOs/PathsFiles/insertQueries_Unix_Paths.json";
            else
                createTablePaths = @".\DAOs\PathsFiles\insertQueries_Unix_Paths.json";
            
            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            UtilsJSON.GetInstance().DeserializeFromString(out _oPathsArray, paths);

        }


        public void SetQuery(string pPath, out Query pQuery)
        {
            string query = UtilsStreamReaders.GetInstance().ReadStreamFile(pPath);
            UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }

        public void InsertUser(User pUser, string[] pTableLine)
        {
            Query oQuery;
            
            this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
            this.PrepareQueryForInsertUser(oQuery, pUser, pTableLine);

            oConnectionPostgreSql.ExecuteCommand(
                oQuery.query.Replace(
                    UtilsConstants._INTERROGANT, oQuery.valuesFormat));
        }

        public void InsertUsers(List<User> pUserList, string[] pTableLine)
        {
            foreach (var oUser in pUserList)
            {
                Query oQuery;
            
                this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
                this.PrepareQueryForInsertUser(oQuery, oUser, pTableLine);
                oConnectionPostgreSql.ExecuteCommand(
                    oQuery.query.Replace(
                        UtilsConstants._INTERROGANT, oQuery.valuesFormat));
            }
        }

        public void InsertKeyPair(KeyPair pKeyPair, string[] pTableLine)
        {
            Query oQuery;
            
            this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
            this.PrepareQueryForInsertKeyPair(oQuery, pKeyPair, pTableLine);
            oConnectionPostgreSql.ExecuteCommand(
                oQuery.query.Replace(UtilsConstants._INTERROGANT, oQuery.valuesFormat));
        }

        public void InsertKeyPair(KeyPair pKeyPair, User pUser, string[] pTableLine)
        {
            Query oQuery;
            
            this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
            this.PrepareQueryForInsertKeyPair(oQuery, pKeyPair, pTableLine);
            oQuery.valuesFormat = oQuery.valuesFormat.Replace(
                UtilsConstants._TWO_QUERY_STRING, pUser.email);
            oConnectionPostgreSql.ExecuteCommand(
                oQuery.query.Replace(UtilsConstants._INTERROGANT, oQuery.valuesFormat));
        }

        private void PrepareQueryForInsertKeyPair(Query pQuery, KeyPair pKeyPair, string[] pTableLine)
        {
            this.PrepareQueryForInsert(pQuery, pTableLine);
            pQuery.valuesFormat = pQuery.valuesFormat.Replace(
                UtilsConstants._ZERO_QUERY_STRING, pKeyPair.public_string);
            pQuery.valuesFormat = pQuery.valuesFormat.Replace(
                UtilsConstants._ONE_QUERY_STRING, pKeyPair.private_string);
        }

        private void PrepareQueryForInsertUser(Query pQuery, User pUser, string[] pTableLine)
        {
            this.PrepareQueryForInsert(pQuery, pTableLine);
            pQuery.valuesFormat = pQuery.valuesFormat.Replace(
                UtilsConstants._ZERO_QUERY_STRING, pUser.email);
            pQuery.valuesFormat = pQuery.valuesFormat.Replace(
                UtilsConstants._ONE_QUERY_STRING, pUser.pass);
        }

        private void PrepareQueryForInsert(Query pQuery, string[] pTableLine)
        {
            pQuery.query = pQuery.query.Replace(
                UtilsConstants._TABLE_NAME, pTableLine[UtilsConstants._ZERO].Trim());
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING), 
                pTableLine[UtilsConstants._ONE].Trim());
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING), 
                pTableLine[UtilsConstants._TWO].Trim());
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._THREE_STRING), 
                pTableLine[UtilsConstants._THREE].Trim());

        }
    }
}