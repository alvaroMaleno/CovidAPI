using System;
using System.Collections.Generic;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.DAOs.Interfaces;
using Security_REST.Models.DataModels;
using Security_REST.Models.QueryModels;
using Security_REST.Utils;

namespace Security_REST.DAOs.SelectTableOperations
{
    public class PostgreSqlSelect : IQuery
    {
        private ConnectionPostgreSql _oConnection { get; set; }

        private readonly string _ONE_STRING = "{1}";
        private readonly string _ZERO_STRING = "{0}";
        private readonly string _TABLE_NAME = "table_name";

        private static PostgreSqlSelect _instance;

        private PostgreSqlSelect(ConnectionPostgreSql pConnection = null)
        {
            if (pConnection is null)
                this._oConnection = new ConnectionPostgreSql();
            else
                this._oConnection = pConnection;
        }

        public static PostgreSqlSelect GetInstance(ConnectionPostgreSql pConnection = null)
        {
            if (_instance is null)
            {
                _instance = new PostgreSqlSelect(pConnection);
            }
            return _instance;
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            UtilsJSON.GetInstance().DeserializeFromString(
                out pQuery,
                UtilsStreamReaders.GetInstance().ReadStreamFile(pPath));
        }

        public void SelectKeyPair(KeyPair pKeyPair, string pTableName)
        {
            throw new NotImplementedException();
        }

        public void SelectAllKeyPairs(List<KeyPair> pKeyPairList, string pTableName)
        {
            throw new NotImplementedException();
        }
        
        public void SelectUser(User pUser, string pTableName)
        {
            throw new NotImplementedException();
        }
        
        public void SelectAllUsers(List<User> pUserList, string pTableName)
        {
            throw new NotImplementedException();
        }
    }
}