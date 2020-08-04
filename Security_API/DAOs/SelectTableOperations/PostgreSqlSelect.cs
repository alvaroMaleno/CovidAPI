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
        private string _ASTERISC = "*";


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

        public void SelectKeyPairFromUser(User pUser, string[] pTableLineout, out KeyPair pKeyPair)
        {
            Query oQuery;
            this.SetQuery(_COLUMN, out oQuery);
            this.PrepareQueryForSelectKeyPairFromColumn(oQuery, pTableLineout, pUser);

            List<object[]> oQueryResult = new List<object[]>();
            _oConnection.ExecuteSelectCommand(oQuery.query, oQueryResult);

            if(oQueryResult.Count < UtilsConstants._ONE)
                pKeyPair = new KeyPair(string.Empty, string.Empty);
            
            pKeyPair = new KeyPair(
                (string) oQueryResult[UtilsConstants._ZERO][UtilsConstants._ZERO], 
                (string) oQueryResult[UtilsConstants._ZERO][UtilsConstants._ONE]);
        }

        private void PrepareQueryForSelectKeyPairFromColumn(Query pQuery, string[] pTableLineout, User pUser)
        {
            this.PrepareQueryForSelectFromColumn(
                pQuery, 
                _ASTERISC, 
                pTableLineout[UtilsConstants._ZERO].Trim(), 
                pTableLineout[UtilsConstants._ONE].Trim());
            pQuery.query = pQuery.query.Replace(UtilsConstants._ZERO_QUERY_STRING, pUser.public_key);
        }

        private void PrepareQueryForSelectFromColumn(
            Query pQuery, 
            string pToSelect, 
            string pTable, 
            string pColumnToCompare)
        {
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING),
                pToSelect);
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO),
                pColumnToCompare);
            pQuery.query = pQuery.query.Replace(UtilsConstants._TABLE_NAME, pTable);
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
        
        public void SelectUser(User pUser, string[] pTableLine, out User pSelectedUser)
        {
            Query oQuery;
            this.SetQuery(_COLUMN, out oQuery);
            this.PrepareQueryForSelectUserFromColumn(oQuery, pTableLine, pUser);

            List<object[]> oQueryResult = new List<object[]>();
            _oConnection.ExecuteSelectCommand(oQuery.query, oQueryResult);

            pSelectedUser = new User();
            
            if(oQueryResult.Count < UtilsConstants._ONE)
            {
                pSelectedUser.email = string.Empty;
                pSelectedUser.pass = string.Empty;
                return;
            }

            pSelectedUser.email = (string)oQueryResult[UtilsConstants._ZERO][UtilsConstants._ZERO];
            pSelectedUser.pass = (string)oQueryResult[UtilsConstants._ZERO][UtilsConstants._ONE];
        }

        private void PrepareQueryForSelectUserFromColumn(Query pQuery, string[] pTableLine, User pUser)
        {
            this.PrepareQueryForSelectFromColumn(
                pQuery, 
                _ASTERISC, 
                pTableLine[UtilsConstants._ZERO].Trim(), 
                pTableLine[UtilsConstants._ONE].Trim());
            pQuery.query = pQuery.query.Replace(UtilsConstants._ZERO_QUERY_STRING, pUser.email);
        }

        public void SelectAllUsers(List<User> pUserList, string pTableName)
        {
            throw new NotImplementedException();
        }
    }
}