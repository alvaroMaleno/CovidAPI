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
            {
                createTablePaths = @"./Processes/InitialDataInsertion/insertData_Unix_Paths.json";
            }
            else
            {
                createTablePaths = @".\Processes\InitialDataInsertion\insertData_Windows_Paths.json";
            }
            
            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            UtilsJSON.GetInstance().DeserializeFromString(out _oPathsArray, paths);

        }


        public void SetQuery(string pPath, out Query pQuery)
        {
            string query = UtilsStreamReaders.GetInstance().ReadStreamFile(pPath);
            UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }

        public void InsertUser(User pUser, string pTableName)
        {
            throw new NotImplementedException();
        }

        public void InsertUsers(List<User> pUserList, string pTableName)
        {
            throw new NotImplementedException();
        }

        public void InsertKeyPair(KeyPair pKeyPair, string pTableName)
        {
            throw new NotImplementedException();
        }
    }
}