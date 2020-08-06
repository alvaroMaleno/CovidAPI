using Security_REST.Controllers.DAOs.Connection;
using Security_REST.DAOs.Interfaces;
using Security_REST.Models.DataModels;
using Security_REST.Models.QueryModels;
using Security_REST.Utils;

namespace Security_REST.DAOs.UpdateTableOperations
{
    public class PostgreSqlUpdate : IQuery
    {
        private ConnectionPostgreSql _oConnection { get; set; }
        private static PostgreSqlUpdate _instance;

        private readonly string _UPDATE_QUERY = "UPDATE table_name SET column_name1 = '{0}' WHERE column_name2 = '{1}'";

        private PostgreSqlUpdate(ConnectionPostgreSql pConnection = null)
        {
            if (pConnection is null)
                this._oConnection = new ConnectionPostgreSql();
            else
                this._oConnection = pConnection;
        }

        public static PostgreSqlUpdate GetInstance(ConnectionPostgreSql pConnection = null)
        {
            if (_instance is null)
            {
                _instance = new PostgreSqlUpdate(pConnection);
            }
            return _instance;
        }

        public void UpdatePublicKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            Query oQuery;
            this.SetQuery(string.Empty, out oQuery);

            this.PrepareQuery(
                oQuery, 
                pTableLine[Utils.UtilsConstants._ZERO].Trim(),
                pTableLine[Utils.UtilsConstants._ONE].Trim(),
                pTableLine[Utils.UtilsConstants._ONE].Trim());

            this.AssignQueryValues(pNewKeyPair.public_string, pOldKeyPair.public_string, oQuery);
            _oConnection.ExecuteCommand(oQuery.query);
        }

        public void UpdatePrivateKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            Query oQuery;
            this.SetQuery(string.Empty, out oQuery);

            this.PrepareQuery(
                oQuery, 
                pTableLine[Utils.UtilsConstants._ZERO].Trim(),
                pTableLine[Utils.UtilsConstants._TWO].Trim(),
                pTableLine[Utils.UtilsConstants._TWO].Trim());

            this.AssignQueryValues(pNewKeyPair.public_string, pOldKeyPair.public_string, oQuery);
            _oConnection.ExecuteCommand(oQuery.query);
        }

        public void UpdatePrivateFromPublicKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            Query oQuery;
            this.SetQuery(string.Empty, out oQuery);

            this.PrepareQuery(
                oQuery, 
                pTableLine[Utils.UtilsConstants._ZERO].Trim(),
                pTableLine[Utils.UtilsConstants._TWO].Trim(),
                pTableLine[Utils.UtilsConstants._ONE].Trim());

            this.AssignQueryValues(pNewKeyPair.private_string, pOldKeyPair.public_string, oQuery);
            _oConnection.ExecuteCommand(oQuery.query);
        }

        private void AssignQueryValues(string pNewValue, string pOldValue, Query pQuery)
        {
            pQuery.query = pQuery.query.Replace(UtilsConstants._ZERO_QUERY_STRING, pNewValue);
            pQuery.query = pQuery.query.Replace(UtilsConstants._ONE_QUERY_STRING, pOldValue);
        }

        private void PrepareQuery(
            Query pQuery, 
            string pTableName, 
            string pColumnOneName,
            string pColumnTwoName)
        {
            pQuery.query = pQuery.query.Replace(UtilsConstants._TABLE_NAME, pTableName);
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING), 
                pColumnOneName);
            pQuery.query = pQuery.query.Replace(
                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING), 
                pColumnTwoName);
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            pQuery = new Query();
            pQuery.query = _UPDATE_QUERY;
        }
    }
}