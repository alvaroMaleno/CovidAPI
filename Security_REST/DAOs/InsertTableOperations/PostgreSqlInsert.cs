using System;
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
        private readonly string _REPLACE_QUERY_CONSTANT = "?";
        private readonly string _REPLACE_SINGLEQUOTE_CONSTANT = "'";
        private readonly string _ONE_STRING = "{1}";
        private readonly string _ZERO_STRING = "{0}";
        private readonly string _COME = ",";
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
            // Query oQuery;
            
            // this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
            // oQuery.query = oQuery.query.Replace(UtilsConstants._TABLE_NAME, pTableName.Trim());
            // oQuery.query = oQuery.query.Replace(
            //     string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING), pUser.email);
            // oQuery.query = oQuery.query.Replace(
            //     string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING), pUser.pass);
            // oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public void InsertUsers(List<User> pUserList, string[] pTableLine)
        {
            // foreach (var oUser in pUserList)
            // {
            //     Query oQuery;
            
            //     this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
            //     oQuery.query = oQuery.query.Replace(UtilsConstants._TABLE_NAME, pTableName.Trim());
            //     oQuery.query = oQuery.query.Replace(
            //         string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING), oUser.email);
            //     oQuery.query = oQuery.query.Replace(
            //         string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING), oUser.pass);
            //     oConnectionPostgreSql.ExecuteCommand(oQuery.query);
            // }
        }

        public void InsertKeyPair(KeyPair pKeyPair, string[] pTableLine)
        {
            Query oQuery;
            
            this.SetQuery(_oPathsArray.oPaths[UtilsConstants._ZERO], out oQuery);
            oQuery.query = oQuery.query.Replace(
                UtilsConstants._TABLE_NAME, pTableLine[UtilsConstants._ZERO].Trim());
            oQuery.query = oQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING), 
                pTableLine[UtilsConstants._ONE].Trim());
            oQuery.query = oQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING), 
                pTableLine[UtilsConstants._TWO].Trim());
            oQuery.query = oQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._THREE_STRING), 
                pTableLine[UtilsConstants._THREE].Trim());
            oQuery.valuesFormat = oQuery.valuesFormat.Replace(
                UtilsConstants._ZERO_QUERY_STRING, 
                string.Concat("'", pKeyPair.public_string, "'"));
            oQuery.valuesFormat = oQuery.valuesFormat.Replace(
                UtilsConstants._ONE_QUERY_STRING, 
                string.Concat("'", pKeyPair.private_string, "'"));
            oQuery.valuesFormat = oQuery.valuesFormat.Replace(
                UtilsConstants._TWO_QUERY_STRING, 
                string.Concat("'", UtilsConstants._TWO_QUERY_STRING, "'"));
            oConnectionPostgreSql.ExecuteCommand(
                oQuery.query.Replace(UtilsConstants._INTERROGANT, oQuery.valuesFormat));
        }
    }
}