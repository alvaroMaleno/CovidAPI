using System;
using System.Collections.Generic;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.DAOs.Interfaces;
using Security_REST.Models.DataModels;
using Security_REST.Models.PathModels;
using Security_REST.Models.QueryModels;
using Security_REST.Utils;

namespace Security_REST.DAOs.SelectTableOperations
{
    public class PostgreSqlSelect : IQuery
    {
        private ConnectionPostgreSql _oConnection { get; set; }
        private static PostgreSqlSelect _instance;
        private Paths _oPaths;
        private string _ALL = "all";
        private string _COLUMN = "column";


        private PostgreSqlSelect(ConnectionPostgreSql pConnection = null)
        {
            if (pConnection is null)
                this._oConnection = new ConnectionPostgreSql();
            else
                this._oConnection = pConnection;

            var file = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(this.GetSelectPath());
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out _oPaths, file);
        }

        private string GetSelectPath()
        {
            string selectPaths;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
                selectPaths = @"./DAOs/PathsFiles/selectQueries_Unix_Paths.json";
            else
                selectPaths = @".\DAOs\PathsFiles\selectQueries_Windows_Paths.json";

            return selectPaths;
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
            string path;
            switch (pPath)
            {
                case "all":
                    path = _oPaths.oPaths[0];
                    break;
                case "column":
                    path = _oPaths.oPaths[1];
                    break;
                default:
                    path = string.Empty;
                    break;
            }
            
            UtilsJSON.GetInstance().DeserializeFromString(
                out pQuery,
                UtilsStreamReaders.GetInstance().ReadStreamFile(path));
        }

        public void SelectKeyPair(KeyPair pKeyPair, string[] pTableLine)
        {
            throw new NotImplementedException();
        }

        public void SelectAllKeyPairs(List<KeyPair> pKeyPairList, string pTableName)
        {
            Query oQuery;
            this.SetQuery(_ALL, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants._TABLE_NAME, pTableName);

            List<object[]> oQueryResult = new List<object[]>();
            _oConnection.ExecuteSelectCommand(oQuery.query, oQueryResult);

            KeyPair oKeyPair;
            foreach (var oRow in oQueryResult)
            {
                oKeyPair = new KeyPair(
                    (string)oRow[UtilsConstants._ZERO], 
                    (string)oRow[UtilsConstants._ONE]);
                pKeyPairList.Add(oKeyPair);
            }
        }
        
        public void SelectUser(User pUser, string[] pTableLine)
        {
            throw new NotImplementedException();
        }
        
        public void SelectAllUsers(List<User> pUserList, string pTableName)
        {
            throw new NotImplementedException();
        }
    }
}